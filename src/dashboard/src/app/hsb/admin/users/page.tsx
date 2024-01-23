'use client';

import styles from './Users.module.scss';

import {
  Button,
  Checkbox,
  Info,
  Overlay,
  Select,
  Sheet,
  Spinner,
  Table,
  Text,
} from '@/components';
import { IUserModel, useAuth } from '@/hooks';
import { useApiUsers } from '@/hooks/api/admin';
import { useGroups, useUsers } from '@/hooks/data';
import { redirect } from 'next/navigation';
import React, { useRef } from 'react';
import { IUserForm } from './IUserForm';

export default function Page() {
  const state = useAuth();
  const { isReady: isReadyUsers, users } = useUsers({ includeGroups: true });
  const { isReady: isReadyGroups, groups, options: groupOptions } = useGroups();
  const { update: updateUser } = useApiUsers();

  const [loading, setLoading] = React.useState(true);
  const [records, setRecords] = React.useState<IUserForm[]>([]);
  const [items, setItems] = React.useState<IUserModel[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const dialogRef = useRef(null);
  const [currentUsername, setCurrentUsername] = React.useState('');
  const [currentEditContext, setCurrentEditContext] = React.useState('');

  const handleEditClick = (username: string, context: string) => {
    setCurrentUsername(username);
    setCurrentEditContext(context);
    if (dialogRef.current) {
      dialogRef.current.showModal();
    }
  };

  const closeDialog = () => {
    if (dialogRef.current) {
      dialogRef.current.close();
    }
  };

  React.useEffect(() => {
    setLoading(!isReadyUsers && !isReadyGroups);
  }, [isReadyUsers, isReadyGroups]);

  React.useEffect(() => {
    setItems(records);
  }, [records]);

  React.useEffect(() => {
    setRecords((state) =>
      users.map((user) => {
        const value = state.find((s) => s.id === user.id);
        if (value) ({ ...user, isDirty: value.isDirty });
        return user;
      }),
    );
    setItems(users);
  }, [users]);

  const handleSearch = React.useCallback(() => {
    setItems(
      filter
        ? records.filter(
            (r) =>
              r.username.includes(filter) ||
              r.displayName.includes(filter) ||
              r.email.includes(filter),
          )
        : records,
    );
  }, [filter, records]);

  const handleUpdate = React.useCallback(async () => {
    const update = records.map(async (user) => {
      if (user.isDirty) {
        try {
          const res = await updateUser(user);
          const result: IUserModel = await res.json();
          return { ...result, isDirty: false };
        } catch (error) {
          console.error(error);
        }
      }
      return user;
    });
    const results = await Promise.all(update);
    setRecords(results);
  }, [updateUser, records]);

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isSystemAdmin) redirect('/');

  return (
    
    <Sheet>
      <dialog ref={dialogRef} className={styles.popup}>
        <p className={styles.popupTitle}>{currentEditContext} for {currentUsername}</p>
        <div className={styles.popupSearch}>
          <Text 
            name="search"
            placeholder="Search"
            iconType="search"
          />
        </div>
        <div className={styles.popupRows}>
          <div className={styles.row}>
            <Checkbox />
            <p>placeholder</p>
          </div>
        </div>
        <div className={styles.popupFooter}>
          <Button variant="secondary" onClick={closeDialog}>Cancel</Button>
          <Button>Save</Button>
        </div>
      </dialog>
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
        data={items}
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
                    setRecords((records) =>
                      records.map((r) =>
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
                  <p><span>Roles: </span>role, role, role</p>
                  <Button variant="secondary" onClick={() => handleEditClick(data.username, 'Editing roles')}>Edit</Button>
                </div>
                <div>
                  <p><span>Organizations: </span>Organization name, Organization name, Organization name</p>
                  <Button variant="secondary" onClick={() => handleEditClick(data.username, 'Allow access to the following organizations')}>Edit</Button>
                </div>
                <div>
                  <p><span>Tenant: </span>Tenant name, Tenant name, Tenant name</p>
                  <Button variant="secondary" onClick={() => handleEditClick(data.username, 'Allow access to the following tenants')}>Edit</Button>
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
          disabled={isSubmitting || !records.some((r) => r.isDirty)}
        >
          Save
        </Button>
      </div>
      </div>
    </Sheet>
  );
}
