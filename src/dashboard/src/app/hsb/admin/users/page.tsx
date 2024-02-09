'use client';

import styles from './Users.module.scss';

import { AdminLoadingAnimation, Button, Checkbox, Info, Sheet, Table, Text } from '@/components';
import { IUserForm, UserDialog, UserDialogVariant } from '@/components/admin';
import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import { useApiUsers } from '@/hooks/api/admin';
import { useGroups, useUsers } from '@/hooks/data';
import { useNavigateStore } from '@/store';
import { searchUsers } from '@/utils';
import { redirect } from 'next/navigation';
import React from 'react';

export default function Page() {
  const state = useAuth();
  const { isReady: isReadyUsers, users } = useUsers({ includePermissions: true, init: true });
  const { isReady: isReadyGroups } = useGroups({ init: true });
  const { update: updateUser } = useApiUsers();
  const setEnableNavigate = useNavigateStore((state) => state.setEnableNavigate);

  const [loading, setLoading] = React.useState(true);
  const [formUsers, setFormUsers] = React.useState<IUserForm[]>([]);
  const [filteredUsers, setFilteredUsers] = React.useState<number[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [newRows, setNewRows] = React.useState<React.ReactNode[]>([]);

  const dialogRef = React.useRef<HTMLDialogElement>(null);
  const [dialog, setDialog] = React.useState<{ user: IUserForm; variant: UserDialogVariant }>();

  const isDirty = formUsers.some((u) => u.isDirty);

  const handleAddNewRow = () => {
    const newRowKey = `newRow-${newRows.length}`;
    const newRowElement = <AddNewRow key={newRowKey} />;
    setNewRows([newRowElement, ...newRows]);
  };  

  React.useEffect(() => {
    setLoading(!isReadyUsers && !isReadyGroups);
  }, [isReadyUsers, isReadyGroups]);

  React.useEffect(() => {
    setEnableNavigate(!isDirty);
  }, [isDirty, setEnableNavigate]);

  React.useEffect(() => {
    setFormUsers((state) =>
      users.map((user) => {
        const value = state.find((s) => s.id === user.id);
        if (value) ({ ...user, isDirty: value.isDirty });
        return user;
      }),
    );
    setFilteredUsers(searchUsers(users, filter).map((u) => u.id));
    // Only apply the 'filter' when the button is pressed.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [users]);

  const handleSearch = React.useCallback(() => {
    setFilteredUsers(searchUsers(formUsers, filter).map((u) => u.id));
  }, [filter, formUsers]);

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

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <AdminLoadingAnimation />;
  if (!state.isSystemAdmin) redirect('/');

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
          <Info>
            Find a user by name, username, or email. Click checkbox to enable user access to
            dashboard for selected group(s).
          </Info>
          <div className={styles.search}>
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
                <div className={styles.tableHeader}>Enabled</div>
                <div>Roles, Tenants, Organizations</div>
              </>
            }
            addNew={
              <>
              <div className={styles.newRow} onClick={handleAddNewRow}>
                <span>+</span> Add new user
              </div>
                {newRows.map(newRow => newRow)}
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
                      checked={data.isEnabled}
                      onChange={(e) => {
                        setFormUsers((formUsers) =>
                          formUsers.map((r) =>
                            r.id === data.id
                              ? { ...data, isEnabled: e.target.checked, isDirty: true }
                              : r,
                          ),
                        );
                      }}
                    />
                  </div>
                  <div className={styles.selectRow}>
                    <div>
                      <p>
                        <span>Roles: </span>
                        {data.groups?.map((org) => org.name).join(', ')}
                      </p>
                      <Button variant="secondary" onClick={() => handleEditClick(data, 'group')}>
                        Edit
                      </Button>
                    </div>
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

const AddNewRow = () => {
  return (
    <div className={styles.addNewRow}>
    <div><Text placeholder="Username" /></div>
    <div><Text placeholder="Email" /></div>
    <div><Text placeholder="Name" /></div>
    <div className={styles.checkbox}><Checkbox /></div>
    <div className={styles.selectRow}>
      <div>
        <p>
          <span>Roles: </span>
        </p>
        <Button variant="secondary">
          Edit
        </Button>
      </div>
      <div>
        <p>
          <span>Organizations: </span>
        </p>
        <Button
          variant="secondary"
        >
          Edit
        </Button>
      </div>
      <div>
        <p>
          <span>Tenant: </span>
        </p>
        <Button variant="secondary">
          Edit
        </Button>
      </div>
    </div>
  </div>
  );
};