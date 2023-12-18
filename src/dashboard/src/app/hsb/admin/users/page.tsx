'use client';

import styles from '@/components/sheet/Sheet.module.scss';

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
import { IUserModel, useApi } from '@/hooks';
import React from 'react';
import { IUserForm } from './IUserForm';

export default function Page() {
  const api = useApi();

  const [loading, setLoading] = React.useState(true);
  const [records, setRecords] = React.useState<IUserForm[]>([]);
  const [items, setItems] = React.useState<IUserModel[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);

  React.useEffect(() => {
    setItems(records);
  }, [records]);

  React.useEffect(() => {
    api
      .findUsers()
      .then(async (response) => {
        const data: IUserModel[] = await response.json();
        setRecords(data.map((d) => ({ ...d, isDirty: false })));
        setItems(data);
      })
      .catch(() => {})
      .finally(() => setLoading(false));
  }, [api]);

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
          const res = await api.updateUser(user);
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
  }, [api, records]);

  return (
    <Sheet>
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
        <Row>
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
        </Row>
      </div>
      <Table
        data={items}
        header={
          <>
            <div>Username</div>
            <div>Email</div>
            <div>Name</div>
            <div>Enabled</div>
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
              <div>
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
              <div>
                <Select options={[]} placeholder="select" />
              </div>
            </>
          );
        }}
      </Table>
      <Row className={styles.footerActions}>
        <Button
          onClick={async () => await handleUpdate()}
          disabled={isSubmitting || !records.some((r) => r.isDirty)}
        >
          Save
        </Button>
      </Row>
    </Sheet>
  );
}
