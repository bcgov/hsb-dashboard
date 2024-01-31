'use client';

import styles from './ClientAdmin.module.scss';

import { Button, Checkbox, Info, Overlay, Select, Sheet, Spinner, Table, Text } from '@/components';
import { IUserModel, RoleName, useAuth } from '@/hooks';
import { useApiUsers } from '@/hooks/api/admin';
import { IOrganizationModel, ITenantModel } from '@/hooks/api/interfaces/auth';
import { useGroups, useTenants, useUsers } from '@/hooks/data';
import { useAppStore } from '@/store';
import { redirect } from 'next/navigation';
import React from 'react';
import { IUserForm } from './IUserForm';
import { getOrganizationOptions, getTenantOptions, searchUsers } from './utils';
import { LoadingAnimation } from '@/components/charts/loadingAnimation';

export default function Page() {
  const state = useAuth();
  const userinfo = useAppStore((state) => state.userinfo);

  const { isReady: isReadyTenants, tenants } = useTenants({ init: true });
  const { isReady: isReadyUsers, users } = useUsers({ includePermissions: true, init: true });
  const { isReady: isReadyGroups, groups } = useGroups({ init: true });
  const { update: updateUser } = useApiUsers();

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

  const handleSearch = React.useCallback(() => {
    setFilteredUsers(searchUsers(formUsers, filter).map((u) => u.id));
  }, [filter, formUsers]);

  const handleUpdate = React.useCallback(async () => {
    const update = formUsers.map(async (user) => {
      if (user.isDirty) {
        try {
          setIsSubmitting(true);
          const res = await updateUser(user);
          const result: IUserModel = await res.json();
          return { ...result, isDirty: false };
        } catch (error) {
          console.error(error);
        } finally {
          setIsSubmitting(false);
        }
      }
      return user;
    });
    const results = await Promise.all(update);
    setFormUsers(results);
  }, [updateUser, formUsers]);

  // Only allow Organization Admin role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isOrganizationAdmin) redirect('/');

  return (
    <Sheet>
      <div className={styles.container}>
        {loading && (
          <LoadingAnimation />
        )}
        <div className={styles.section}>
          <div className={styles.search}>
            <Select
              options={tenantOptions}
              placeholder="Select Tenant"
              value={tenantOptions.find((o) => o.value == selectedTenant?.id)?.value ?? ''}
              onChange={(value) => {
                const tenant = userinfo?.tenants.find((t) => t.id == value);
                setSelectedTenant(tenant);
              }}
            />
            {/* <Select
              options={organizationOptions}
              placeholder="Select Organization"
              value={
                organizationOptions.find((o) => o.value == selectedOrganization?.id)?.value ?? ''
              }
              onChange={(value) => {
                const organization = userinfo?.organizations.find((t) => t.id == value);
                setSelectedOrganization(organization);
              }}
            /> */}
            <Text
              name="search"
              placeholder="Search"
              iconType="search"
              onChange={(e) => setFilter(e.target.value)}
              onKeyDown={(e) => {
                if (e.code === 'Enter') handleSearch();
              }}
            />
            <Button variant="secondary" onClick={() => handleSearch()}>
              Search
            </Button>
          </div>
          <Info>
            Enable access to the dashboard and/or admin access to users associated with{' '}
            {organization}:
          </Info>
        </div>
        <div className={styles.table}>
          <Table
            data={
              selectedTenant || selectedOrganization
                ? formUsers.filter((u) => filteredUsers.some((fu) => fu === u.id))
                : []
            }
            header={
              <>
                <div>Access</div>
                <div>Username</div>
                <div>Email</div>
                <div>Name</div>
                <div>Admin</div>
              </>
            }
          >
            {({ data }) => {
              return (
                <>
                  <div className={styles.checkbox}>
                    <Checkbox
                      checked={data.tenants?.some((t) => t.id === selectedTenant?.id)}
                      onChange={(e) => {
                        // Add/Remove the organization from the user.
                        setFormUsers((formUsers) =>
                          formUsers.map((r) => {
                            const tenant = tenants.find((t) => t.id === selectedTenant?.id);
                            const userTenants =
                              e.target.checked && tenant
                                ? [...(r.tenants ?? []), tenant]
                                : r.tenants?.filter((t) => t.id !== tenant?.id) ?? [];
                            return r.id === data.id
                              ? { ...data, tenants: userTenants, isDirty: true }
                              : r;
                          }),
                        );
                      }}
                    />
                  </div>
                  <div>{data.username}</div>
                  <div>{data.email}</div>
                  <div>{data.displayName}</div>
                  <div className={styles.checkbox}>
                    <Checkbox
                      disabled={!data.tenants?.some((t) => t.id === selectedTenant?.id)}
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
