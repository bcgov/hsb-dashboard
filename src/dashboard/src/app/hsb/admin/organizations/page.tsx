'use client';

import styles from './Organizations.module.scss';

import {
  Button,
  Checkbox,
  Info,
  Overlay,
  Sheet,
  Spinner,
  Table,
  Text,
} from '@/components';
import { IUserModel, useAuth } from '@/hooks';
import { redirect } from 'next/navigation';
import { useApiUsers } from '@/hooks/api/admin';
import { useGroups, useUsers } from '@/hooks/data';
import React from 'react';
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

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isSystemAdmin) redirect('/');

  return (
    <div className={styles.panelContainer}>
    <Sheet >
      <div className={styles.container}>
      {loading && (
        <Overlay>
          <Spinner />
        </Overlay>
      )}
      <div className={styles.section}>
        <Info>
          Find an organization by name and/or associated tenant.  Click checkbox to enable  or disable organization on dashboard.
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
      <div>
      <Table
        data={items}
        header={
          <>
            <div>Organization Name</div>
            <div>Associated Tenants</div>
            <div>Dashboard Enabled</div>
          </>
        }
      >
        {({ data }) => {
          return (
            <>
              <div>organization name</div>
              <div>tenant name</div>
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
          disabled={isSubmitting || !records.some((r) => r.isDirty)}
        >
          Save
        </Button>
      </div>
      </div>
    </Sheet>
    </div>
  );
}
