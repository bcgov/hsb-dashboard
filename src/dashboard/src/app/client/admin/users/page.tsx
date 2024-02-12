'use client';

import styles from './ClientAdmin.module.scss';

import {
  AdminLoadingAnimation,
  Button,
  Checkbox,
  Info,
  Select,
  Sheet,
  Table,
  Text,
} from '@/components';
import { IUserForm, UserDialog, UserDialogVariant } from '@/components/admin';
import { LoadingAnimation } from '@/components/loadingAnimation';
import { IUserModel, RoleName, useAuth } from '@/hooks';
import { useApiUsers } from '@/hooks/api/admin';
import { IOrganizationModel, ITenantModel } from '@/hooks/api/interfaces/auth';
import { useGroups, useTenants, useUsers } from '@/hooks/data';
import { useAppStore, useNavigateStore } from '@/store';
import { redirect } from 'next/navigation';
import React from 'react';
import { getOrganizationOptions, getTenantOptions, searchUsers } from './utils';

export default function Page() {
  const state = useAuth();
  const userinfo = useAppStore((state) => state.userinfo);
  const setEnableNavigate = useNavigateStore((state) => state.setEnableNavigate);

  const { isReady: isReadyTenants } = useTenants({ init: true });
  const { isReady: isReadyUsers, users } = useUsers({ includePermissions: true, init: true });
  const { isReady: isReadyGroups, groups } = useGroups({ init: true });
  const { update: updateUser } = useApiUsers();

  const dialogRef = React.useRef<HTMLDialogElement>(null);
  const [dialog, setDialog] = React.useState<{ user: IUserForm; variant: UserDialogVariant }>();
  const [loading, setLoading] = React.useState(true);
  const [selectedTenant, setSelectedTenant] = React.useState<ITenantModel>();
  const [tenantOptions, setTenantOptions] = React.useState(
    getTenantOptions(userinfo?.tenants ?? []),
  );
  const [selectedOrganization, setSelectedOrganization] = React.useState<IOrganizationModel>();
  const [organizationOptions, setOrganizationOptions] = React.useState(
    getOrganizationOptions(userinfo?.organizations ?? []),
  );
  const [formUsers, setFormUsers] = React.useState<IUserForm[]>([]);
  const [filteredUsers, setFilteredUsers] = React.useState<number[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [organization, setOrganization] = React.useState('');

  const isDirty = formUsers.some((u) => u.isDirty);

  React.useEffect(() => {
    setEnableNavigate(!isDirty);
  }, [isDirty, setEnableNavigate]);

  React.useEffect(() => {
    setLoading(!isReadyUsers || !isReadyTenants || !isReadyGroups);
  }, [isReadyUsers, isReadyTenants, isReadyGroups]);

  React.useEffect(() => {
    setTenantOptions(getTenantOptions(userinfo?.tenants ?? []));
  }, [userinfo?.tenants]);

  React.useEffect(() => {
    setOrganizationOptions(getOrganizationOptions(userinfo?.organizations ?? []));
  }, [userinfo?.organizations]);

  React.useEffect(() => {
    setFormUsers((state) =>
      users.map((user) => {
        const value = state.find((s) => s.id === user.id);
        if (value) ({ ...user, isDirty: value.isDirty });
        return user;
      }),
    );
    setFilteredUsers(searchUsers(users, filter).map((u) => u.id));
    setOrganization('Example Organization');
    // Only apply the 'filter' when the button is pressed.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [users]);

  const doSearch = React.useCallback(
    (
      users: IUserModel[],
      search?: string,
      tenant?: ITenantModel,
      organization?: IOrganizationModel,
    ) => {
      setFilteredUsers(searchUsers(users, search, tenant, organization).map((u) => u.id));
    },
    [],
  );

  const handleUpdate = React.useCallback(async () => {
    setIsSubmitting(true);
    const update = formUsers.map(async (user) => {
      if (user.isDirty) {
        let result = user;
        try {
          setFormUsers((users) =>
            users.map((fu) => (fu.id === user.id ? { ...result, isSubmitting: true } : fu)),
          );
          const res = await updateUser(user);
          result = await res.json();
          return { ...result, isDirty: false };
        } catch (error) {
          console.error(error);
        } finally {
          setFormUsers((users) =>
            users.map((fu) => (fu.id === user.id ? { ...result, isSubmitting: false } : fu)),
          );
        }
      }
      return user;
    });
    const results = await Promise.all(update);
    setIsSubmitting(false);
    setFormUsers((users) =>
      users.map((user) => {
        return results.find((u) => u.id === user.id) ?? user;
      }),
    );
  }, [updateUser, formUsers]);

  const handleEditClick = (user: IUserForm, variant: UserDialogVariant) => {
    setDialog({ user, variant });
    dialogRef.current?.showModal();
  };

  // Only allow Organization Admin role to view this page.
  if (state.status === 'loading') return <AdminLoadingAnimation />;
  if (!state.isOrganizationAdmin) redirect('/');

  return (
    <Sheet>
      <UserDialog
        ref={dialogRef}
        user={dialog?.user}
        variant={dialog?.variant}
        onChange={(data) =>
          setFormUsers((formUsers) =>
            formUsers.map((u) => (u.id === data.id ? { ...data, isDirty: true } : u)),
          )
        }
        onSave={async () => {
          dialogRef.current?.close();
          await handleUpdate();
        }}
      />
      <div className={styles.container}>
        {loading && <LoadingAnimation />}
        <div className={styles.section}>
          <div className={styles.search}>
            <Text
              name="search"
              placeholder="Search"
              iconType="search"
              onChange={(e) => setFilter(e.target.value)}
              onKeyDown={(e) => {
                if (e.code === 'Enter')
                  doSearch(formUsers, filter, selectedTenant, selectedOrganization);
              }}
            />
            <Button
              variant="secondary"
              onClick={() => doSearch(formUsers, filter, selectedTenant, selectedOrganization)}
            >
              Search
            </Button>
            {!!tenantOptions.length && (
              <Select
                options={tenantOptions}
                placeholder="Select Tenant"
                value={tenantOptions.find((o) => o.value == selectedTenant?.id)?.value ?? ''}
                onChange={(value) => {
                  const tenant = userinfo?.tenants.find((t) => t.id == value);
                  setSelectedTenant(tenant);
                  doSearch(formUsers, filter, tenant, selectedOrganization);
                }}
              />
            )}
            {!!organizationOptions.length && (
              <Select
                options={organizationOptions}
                placeholder="Select Organization"
                value={
                  organizationOptions.find((o) => o.value == selectedOrganization?.id)?.value ?? ''
                }
                onChange={(value) => {
                  const organization = userinfo?.organizations.find((t) => t.id == value);
                  setSelectedOrganization(organization);
                  doSearch(formUsers, filter, selectedTenant, organization);
                }}
              />
            )}
          </div>
          <Info>
            Enable user access to tenants and/or organizations, and make users administrators.
            {organization}:
          </Info>
        </div>
        <div className={styles.table}>
          <Table
            rows={formUsers
              .filter((u) => filteredUsers.some((user) => user === u.id))
              .map((user, index) => ({ data: user, index, loading: user.isSubmitting }))}
            header={
              <>
                <div>Username</div>
                <div>Email</div>
                <div>Name</div>
                <div className={styles.tableHeader}>Admin</div>
                <div>Roles, Tenants, Organizations</div>
              </>
            }
          >
            {({ data }) => {
              return (
                <>
                  <div>{data.username}</div>
                  <div>{data.email}</div>
                  <div>{data.displayName}</div>
                  <div className={styles.checkbox}>
                    <Checkbox
                      checked={data.groups?.some((t) => t.name === RoleName.OrganizationAdmin)}
                      onChange={(e) => {
                        // Add/Remove organization-admin group.
                        setFormUsers((formUsers) => {
                          const group = groups.find((g) => g.name === RoleName.OrganizationAdmin);
                          const userGroups =
                            e.target.checked && group
                              ? [...(data.groups ?? []), group]
                              : data.groups?.filter((g) => g.name !== RoleName.OrganizationAdmin) ??
                                [];
                          return formUsers.map((r) =>
                            r.id === data.id ? { ...data, groups: userGroups, isDirty: true } : r,
                          );
                        });
                      }}
                    />
                  </div>
                  <div className={styles.selectRow}>
                    <div>
                      <p>
                        <span>Organizations: </span>
                        {data.organizations?.map((org) => org.name).join(', ')}
                      </p>
                      <Button
                        variant="secondary"
                        onClick={() => handleEditClick(data, 'organization')}
                      >
                        Edit
                      </Button>
                    </div>
                    <div>
                      <p>
                        <span>Tenant: </span>
                        {data.tenants?.map((org) => org.name).join(', ')}
                      </p>
                      <Button variant="secondary" onClick={() => handleEditClick(data, 'tenant')}>
                        Edit
                      </Button>
                    </div>
                  </div>
                </>
              );
            }}
          </Table>
        </div>
        <div className={styles.footer}>
          <Button
            onClick={async () => await handleUpdate()}
            disabled={isSubmitting || !formUsers.some((r) => r.isDirty)}
          >
            Save
          </Button>
        </div>
      </div>
    </Sheet>
  );
}
