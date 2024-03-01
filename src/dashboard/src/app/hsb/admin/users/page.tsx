'use client';

import styles from './Users.module.scss';

import { AdminLoadingAnimation, Button, Info, Sheet, Table, Text } from '@/components';
import { IUserForm, UserDialog, UserDialogVariant } from '@/components/admin';
import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import { useAdminGroups, useAdminUsers } from '@/hooks/admin';
import { useApiUsers } from '@/hooks/api/admin';
import { useNavigateStore } from '@/store';
import { searchUsers } from '@/utils';
import { redirect } from 'next/navigation';
import React from 'react';
import { toast } from 'react-toastify';
import { AddNewUserRow } from './AddNewUserRow';
import { EditUserRow } from './EditUserRow';
import { defaultUser } from './defaultUser';
import { validateUser } from './validateUser';

export default function Page() {
  const state = useAuth();
  const { isReady: isReadyUsers, users } = useAdminUsers({ includePermissions: true, init: true });
  const { isReady: isReadyGroups } = useAdminGroups({ init: true });
  const { update: updateUser, add: addUser } = useApiUsers();
  const setEnableNavigate = useNavigateStore((state) => state.setEnableNavigate);

  const [loading, setLoading] = React.useState(true);
  const [formUsers, setFormUsers] = React.useState<IUserForm[]>([]);
  const [filteredUsers, setFilteredUsers] = React.useState<number[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [errors, setErrors] = React.useState<{
    [key: string]: { [K in keyof IUserForm]?: string };
  }>({});

  const dialogRef = React.useRef<HTMLDialogElement>(null);
  const [dialog, setDialog] = React.useState<{ user: IUserForm; variant: UserDialogVariant }>();

  const isDirty = formUsers.some((u) => u.isDirty);

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

  const handleSearch = React.useCallback(
    (users: IUserForm[]) => {
      setFilteredUsers(searchUsers(users, filter).map((u) => u.id));
    },
    [filter],
  );

  const handleAddNewRow = React.useCallback((users: IUserForm[]) => {
    var newUsers = [defaultUser(), ...users];
    setFormUsers(newUsers);
    searchUsers(newUsers);
  }, []);

  const handleUpdate = React.useCallback(async () => {
    setIsSubmitting(true);
    const update = formUsers.map(async (user) => {
      if (user.isDirty) {
        if (validateUser(user, setErrors)) {
          let result = user;
          try {
            setFormUsers((users) =>
              users.map((fu) => (fu.key === user.key ? { ...result, isSubmitting: true } : fu)),
            );
            const res = user.id ? await updateUser(user) : await addUser(user);
            result = await res.json();
            return { ...result, isDirty: false };
          } catch (ex) {
            const error = ex as Error;
            toast.error(error.message);
            console.error(error);
          } finally {
            setFormUsers((users) =>
              users.map((fu) => (fu.key === user.key ? { ...result, isSubmitting: false } : fu)),
            );
          }
        }
      }
      return user;
    });
    const results = await Promise.all(update);
    setIsSubmitting(false);
    setFormUsers((users) =>
      users.map((user) => {
        return results.find((u) => u.key === user.key) ?? user;
      }),
    );
    handleSearch(results);
  }, [formUsers, handleSearch, updateUser, addUser]);

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
        cancelLabel="Close"
        onChange={(data) =>
          setFormUsers((formUsers) =>
            formUsers.map((u) => (u.key === data.key ? { ...data, isDirty: true } : u)),
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
                if (e.code === 'Enter') handleSearch(formUsers);
              }}
            />
            <Button variant="secondary" onClick={() => handleSearch(formUsers)}>
              Search
            </Button>
          </div>
        </div>

        <div className={styles.table}>
          <Table
            rowKey={(item) => item.key}
            rows={formUsers
              .filter((u) => !u.id || filteredUsers.some((userId) => userId === u.id))
              .map((user, index) => ({
                data: user,
                index,
                loading: user.isSubmitting,
              }))}
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
              <div className={styles.newRow} onClick={() => handleAddNewRow(formUsers)}>
                <span>+</span> Add new user
              </div>
            }
          >
            {({ data, index }) => {
              if (!data.id) {
                return (
                  <AddNewUserRow
                    index={index}
                    values={data}
                    errors={errors[data.key]}
                    onChange={(values) => {
                      setFormUsers((users) =>
                        users.map((u) => (u.key === values.key ? values : u)),
                      );
                    }}
                    onEditGroups={(values) => handleEditClick(values, 'group')}
                    onEditOrganizations={(values) => handleEditClick(values, 'organization')}
                    onEditTenants={(values) => handleEditClick(values, 'tenant')}
                  />
                );
              } else {
                return (
                  <EditUserRow
                    index={index}
                    values={data}
                    onChange={(values) => {
                      setFormUsers((users) =>
                        users.map((u, i) => (u.id === values.id ? values : u)),
                      );
                    }}
                    onEditGroups={(values) => handleEditClick(values, 'group')}
                    onEditOrganizations={(values) => handleEditClick(values, 'organization')}
                    onEditTenants={(values) => handleEditClick(values, 'tenant')}
                  />
                );
              }
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
