'use client';

import { AllocationTable } from '@/components';
import { useAuth } from '@/hooks';
import { useOperatingSystemItems, useServerItems } from '@/hooks/data';
import { useDashboardStore, useFilteredStore } from '@/store';
import { redirect, useRouter } from 'next/navigation';
import { LoadingAnimation } from '@/components/charts/loadingAnimation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { isReady: isReadyOperatingSystemItems } = useOperatingSystemItems({ init: true });
  const { isReady: isReadyServerItems, serverItems } = useServerItems({
    useSimple: true,
    init: true,
  });
  const setFilteredTenant = useFilteredStore((state) => state.setTenant);
  const setFilteredOrganization = useFilteredStore((state) => state.setOrganization);
  const setFilteredOperatingSystemItem = useFilteredStore((state) => state.setOperatingSystemItem);
  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);
  const setDashboardServerItems = useDashboardStore((state) => state.setServerItems);

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isClient) redirect('/');

  return (
    <AllocationTable
      margin={-75}
      serverItems={serverItems}
      loading={!isReadyOperatingSystemItems && !isReadyServerItems}
      onClick={(serverItem) => {
        if (serverItem) {
          setFilteredTenant();
          setFilteredOrganization();
          setFilteredOperatingSystemItem();
          setFilteredServerItem(serverItem);
          setDashboardServerItems([serverItem]);
          router.push(`/client/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
    />
  );
}
