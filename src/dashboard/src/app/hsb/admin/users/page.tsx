'use client';
import { useApi } from '@/app/hooks';
import React from 'react';

export default function Page() {
  const api = useApi();

  React.useEffect(() => {
    api
      .findUsers({ username: 'test' })
      .then(async (response) => {
        const data = await response.json();
        console.debug(data);
      })
      .catch(() => {});
  }, [api]);
  return <div>HSB User Admin</div>;
}
