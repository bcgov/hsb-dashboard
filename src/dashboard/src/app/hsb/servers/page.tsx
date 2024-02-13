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
  const { operatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const { isReady, serverItems } = useServerItems({ useSimple: true, init: true });
  const setValues = useFilteredStore((state) => state.setValues);
  const setDashboardServerItems = useDashboardStore((state) => state.setServerItems);

  const updateDashboard = useDashboardFilter();

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isHSB) redirect('/');

  return (
    <AllocationTable
      margin={-75}
      serverItems={serverItems}
      loading={!isReady}
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
          router.push(`/hsb/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
    />
  );
}
