import { useDashboardStore } from '@/store';
import React from 'react';

export const useDashboardOrganizations = () => {
  const organization = useDashboardStore((state) => state.organization);
  const organizations = useDashboardStore((state) => state.organizations);
  const setOrganizations = useDashboardStore((state) => state.setOrganizations);

  return React.useMemo(
    () => ({ organization, organizations, setOrganizations }),
    [organization, organizations, setOrganizations],
  );
};
