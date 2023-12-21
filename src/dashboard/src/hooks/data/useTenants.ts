import { useApp } from '@/store';
import React from 'react';
import { ITenantModel, useApiTenants, useAuth } from '..';

export const useTenants = () => {
  const { status } = useAuth();
  const { findTenants } = useApiTenants();
  const tenants = useApp((state) => state.tenants);
  const setTenants = useApp((state) => state.setTenants);

  React.useEffect(() => {
    // Get an array of tenants.
    if (status === 'authenticated' && !tenants.length) {
      findTenants()
        .then(async (res) => {
          const tenants: ITenantModel[] = await res.json();
          setTenants(tenants);
        })
        .catch((error) => {
          console.error(error);
        });
    }
  }, [findTenants, setTenants, status, tenants.length]);

  return tenants;
};
