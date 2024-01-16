import { useDashboard } from '@/store';
import React from 'react';

export const useDashboardOrganizations = () => {
  const organizations = useDashboard((state) => state.organizations);
  const setOrganizations = useDashboard((state) => state.setOrganizations);

  return React.useMemo(
    () => ({
      organizations,
      setOrganizations,
    }),
    [organizations, setOrganizations],
  );
};
