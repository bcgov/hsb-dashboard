'use client';

import styles from './ClientAdmin.module.scss';

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
import { redirect } from 'next/navigation';
import { useApiUsers } from '@/hooks/api/admin';
import { useUsers } from '@/hooks/data';
import React from 'react';
import { IUserForm } from './IUserForm';

export default function Page() {
  const state = useAuth();

  // Only allow Organization Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isOrganizationAdmin) redirect('/');

  const { isReady: isReadyUsers, users } = useUsers({ includeGroups: trur });
  const { update: updateUser } = useApiUsers();

  const [loading, setLoading] = React.useState(true);
  const [records, setRecords] = React.useState<IUserForm[]>([]);
  const [items, setItems] = React.useState<IUserModel[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);
  const [organization, setOrganization] = React.useState('');

  React.useEffect(() => {
    setLoading(!isReadyUsers);
  }, [isReadyUsers]);

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
    setOrganization('Example Organization');
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

  return (
    <Sheet >
      <div className={styles.container}>
      {loading && (
        <Overlay>
          <Spinner />
        </Overlay>
      )}
      <div className={styles.section}>
        <div className={styles.search}>
          <Select 
            options={[]}
            placeholder="Select Organization" 
          />
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
        Enable access to the dashboard and/or admin access to users associated with {organization}:
        </Info>
      </div>
      <div className={styles.table}>
      <Table
        data={items}
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
            </>
          );
        }}
      </Table>
      </div>
      <div className={styles.footer}>
        <Button
          onClick={async () => await handleUpdate()}
          disabled={isSubmitting || !records.some((r) => r.isDirty)}>
          Save
        </Button>
      </div>
      </div>
    </Sheet>
  );
}
