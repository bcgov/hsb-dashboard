'use client';

import styles from './Users.module.scss';

import {
  Button,
  Checkbox,
  ConfirmPopup,
  Info,
  Overlay,
  Sheet,
  Spinner,
  Table,
  Text,
} from '@/components';
import { IUserModel, useAuth } from '@/hooks';
import { useApiUsers } from '@/hooks/api/admin';
import { useGroups, useUsers } from '@/hooks/data';
import { searchUsers } from '@/utils';
import { redirect } from 'next/navigation';
import React, { useRef } from 'react';
import { IUserForm } from './IUserForm';
import { UserDialog, UserDialogVariant } from './UserDialog';

export default function Page() {
  const state = useAuth();
  const { isReady: isReadyUsers, users } = useUsers({ includePermissions: true, init: true });
  const { isReady: isReadyGroups } = useGroups({ init: true });
  const { update: updateUser } = useApiUsers();

  const [loading, setLoading] = React.useState(true);
  const [formUsers, setFormUsers] = React.useState<IUserForm[]>([]);
  const [filteredUsers, setFilteredUsers] = React.useState<number[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);

  const dialogRef = useRef<HTMLDialogElement>(null);
  const [dialog, setDialog] = React.useState<{ user: IUserForm; variant: UserDialogVariant }>();
  const [showPopup, setShowPopup] = React.useState(false);

  React.useEffect(() => {
    setLoading(!isReadyUsers && !isReadyGroups);
  }, [isReadyUsers, isReadyGroups]);

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
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isSystemAdmin) redirect('/');

  return (
    <Sheet>
      {showPopup && (
        <ConfirmPopup
          onCancel={() => setShowPopup(false)}
          onConfirm={() => {
            setShowPopup(false);
          }}
        />
      )}
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
          await handleUpdate();
          dialogRef.current?.close();
        }}
      />
      <div className={styles.container}>
        {loading && (
          <Overlay>
            <Spinner />
          </Overlay>
        )}
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
            data={formUsers.filter((u) => filteredUsers.some((fu) => fu === u.id))}
            header={
              <>
                <div>Username</div>
                <div>Email</div>
                <div>Name</div>
                <div className={styles.tableHeader}>Enabled</div>
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
                        <span>Roles: </span>role, role, role
                      </p>
                      <Button variant="secondary" onClick={() => handleEditClick(data, 'group')}>
                        Edit
                      </Button>
                    </div>
                    <div>
                      <p>
                        <span>Organizations: </span>Organization name, Organization name,
                        Organization name
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
                        <span>Tenant: </span>Tenant name, Tenant name, Tenant name
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
