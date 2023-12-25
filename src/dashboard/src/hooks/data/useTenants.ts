import { useApp } from '@/store';
import React from 'react';
import { ITenantModel, useApiTenants, useAuth } from '..';

export const useTenants = () => {
  const { status } = useAuth();
  const { find } = useApiTenants();
  const tenants = useApp((state) => state.tenants);
  const setTenants = useApp((state) => state.setTenants);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of tenants.
    if (status === 'authenticated' && !tenants.length) {
      setIsReady(false);
      find()
        .then(async (res) => {
          const tenants: ITenantModel[] = await res.json();
          setTenants(tenants);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (tenants.length) setIsReady(true);
  }, [find, setTenants, status, tenants.length]);

  return { isReady, tenants };
};
