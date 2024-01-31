'use client';

import { AllocationTable } from '@/components';
import { useAuth } from '@/hooks';
import { useServerItems } from '@/hooks/data';
import { useDashboardStore, useFilteredStore } from '@/store';
import { redirect, useRouter } from 'next/navigation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { isReady, serverItems } = useServerItems({ useSimple: true, init: true });
  const setFilteredTenant = useFilteredStore((state) => state.setTenant);
  const setFilteredOrganization = useFilteredStore((state) => state.setOrganization);
  const setFilteredOperatingSystemItem = useFilteredStore((state) => state.setOperatingSystemItem);
  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);
  const setDashboardServerItems = useDashboardStore((state) => state.setServerItems);

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isHSB) redirect('/');

  return (
    <AllocationTable
      margin={-75}
      serverItems={serverItems}
      loading={!isReady}
      onClick={(serverItem) => {
        if (serverItem) {
          setFilteredTenant();
          setFilteredOrganization();
          setFilteredOperatingSystemItem();
          setFilteredServerItem(serverItem);
          setDashboardServerItems([serverItem]);
          router.push(`/hsb/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
    />
  );
}
