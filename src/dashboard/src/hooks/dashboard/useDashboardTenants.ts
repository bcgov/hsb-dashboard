import { useDashboardStore } from '@/store';
import React from 'react';

export const useDashboardTenants = () => {
  const tenant = useDashboardStore((state) => state.tenant);
  const tenants = useDashboardStore((state) => state.tenants);
  const setTenants = useDashboardStore((state) => state.setTenants);

  return React.useMemo(
    () => ({
      tenant,
      tenants,
      setTenants,
    }),
    [tenant, tenants, setTenants],
  );
};
