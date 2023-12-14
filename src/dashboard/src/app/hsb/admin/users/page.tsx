'use client';

import { Table } from '@/components';
import { IUserModel, useApi } from '@/hooks';
import React from 'react';

export default function Page() {
  const api = useApi();

  const [records, setRecords] = React.useState<IUserModel[]>([]);

  React.useEffect(() => {
    api
      .findUsers()
      .then(async (response) => {
        const data = await response.json();
        setRecords(data);
      })
      .catch(() => {});
  }, [api]);

  return (
    <div>
      HSB User Admin
      <Table
        data={records}
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
              <div>{data.isEnabled}</div>
              <div>{data.groups.length}</div>
            </>
          );
        }}
      </Table>
    </div>
  );
}
