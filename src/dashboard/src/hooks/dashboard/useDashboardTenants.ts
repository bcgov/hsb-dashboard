import { useDashboard } from '@/store';
import React from 'react';

export const useDashboardTenants = () => {
  const tenants = useDashboard((state) => state.tenants);
  const setTenants = useDashboard((state) => state.setTenants);

  return React.useMemo(
    () => ({
      tenants,
      setTenants,
    }),
    [tenants, setTenants],
  );
};
