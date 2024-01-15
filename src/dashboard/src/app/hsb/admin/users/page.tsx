'use client';

import styles from './Users.module.scss';

import {
  Button,
  Checkbox,
  Info,
  Overlay,
  Row,
  Select,
  Sheet,
  Spinner,
  Table,
  Text,
} from '@/components';
import { IUserModel } from '@/hooks';
import { useApiUsers } from '@/hooks/api/admin';
import { useGroups, useUsers } from '@/hooks/data';
import React from 'react';
import { IUserForm } from './IUserForm';

export default function Page() {
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
        <Info>
          Find a user by name, username, or email. Click checkbox to enable user access to dashboard
          for selected group(s).
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
            <div>Groups</div>
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
              <div className={styles.groupsSelect}>
                <Select
                  options={groupOptions}
                  className={styles.multiSelect}
                  placeholder="Select one or more"
                  multiple
                  value={data.groups?.map((g) => g.id.toString())}
                  onChange={(values) => {
                    if (Array.isArray(values)) {
                      setRecords((users) =>
                        users.map((u) =>
                          u.id === data.id
                            ? {
                                ...u,
                                groups: groups.filter((g) => values?.some((v) => v == g.id)),
                                isDirty: true,
                              }
                            : u,
                        ),
                      );
                    }
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
          disabled={isSubmitting || !records.some((r) => r.isDirty)}
        >
          Save
        </Button>
      </div>
      </div>
    </Sheet>
  );
}
