import { useDashboardStore } from '@/store';
import React from 'react';

export const useDashboard = () => {
  const tenant = useDashboardStore((state) => state.tenant);
  const organization = useDashboardStore((state) => state.organization);
  const operatingSystemItem = useDashboardStore((state) => state.operatingSystemItem);
  const serverItem = useDashboardStore((state) => state.serverItem);

  return React.useMemo(
    () => ({
      tenantId: tenant?.id,
      organizationId: organization?.id,
      operatingSystemItemId: operatingSystemItem?.id,
      serverItemKey: serverItem?.serviceNowKey,
    }),
    [operatingSystemItem?.id, organization?.id, serverItem?.serviceNowKey, tenant?.id],
  );
};
