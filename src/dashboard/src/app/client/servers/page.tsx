'use client';

import { AllocationTable } from '@/components';
import { useAuth } from '@/hooks';
import { useOperatingSystemItems, useServerItems } from '@/hooks/data';
import { useDashboard, useFiltered } from '@/store';
import { redirect, useRouter } from 'next/navigation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { isReady: isReadyOperatingSystemItems } = useOperatingSystemItems({ init: true });
  const { isReady: isReadyServerItems, serverItems } = useServerItems({
    useSimple: true,
    init: true,
  });
  const setFilteredTenant = useFiltered((state) => state.setTenant);
  const setFilteredOrganization = useFiltered((state) => state.setOrganization);
  const setFilteredOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const setFilteredServerItem = useFiltered((state) => state.setServerItem);
  const setDashboardServerItems = useDashboard((state) => state.setServerItems);

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
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
