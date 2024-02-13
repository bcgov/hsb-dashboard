'use client';

import { AllocationTable, useDashboardFilter } from '@/components';
import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/data';
import { useDashboardStore, useFilteredStore } from '@/store';
import { redirect, useRouter } from 'next/navigation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { tenants } = useTenants({ init: true });
  const { organizations } = useOrganizations({
    init: true,
    includeTenants: true,
  });
  const { isReady: isReadyOperatingSystemItems, operatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const { isReady: isReadyServerItems, serverItems } = useServerItems({
    useSimple: true,
    init: true,
  });
  const setValues = useFilteredStore((state) => state.setValues);
  const setDashboardServerItems = useDashboardStore((state) => state.setServerItems);

  const updateDashboard = useDashboardFilter();

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isClient) redirect('/');

  return (
    <AllocationTable
      margin={-75}
      serverItems={serverItems}
      loading={!isReadyOperatingSystemItems && !isReadyServerItems}
      onClick={async (serverItem) => {
        if (serverItem) {
          const tenant = tenants.find((tenant) => tenant.id === serverItem?.tenantId);
          const organization = organizations.find(
            (organization) => organization.id === serverItem?.organizationId,
          );
          const operatingSystemItem = operatingSystemItems.find(
            (operatingSystemItem) => operatingSystemItem.id === serverItem?.operatingSystemItemId,
          );
          setValues((state) => ({ serverItem, tenant, organization, operatingSystemItem }));
          await updateDashboard({
            tenant,
            organization,
            operatingSystemItem,
            serverItem,
            applyFilter: true,
          });
          router.push(`/client/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
    />
  );
}
