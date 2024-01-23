'use client';

import { AllocationTable, Col } from '@/components';
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
  const setFilteredServerItem = useFiltered((state) => state.setServerItem);
  const setDashboardServerItems = useDashboard((state) => state.setServerItems);

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isClient) redirect('/');

  return (
    <Col>
      <h1>All Servers</h1>
      <AllocationTable
        serverItems={serverItems}
        loading={!isReadyOperatingSystemItems && !isReadyServerItems}
        onClick={(serverItem) => {
          if (serverItem) {
            setFilteredServerItem(serverItem);
            setDashboardServerItems([serverItem]);
            router.push(`/client/dashboard?serverItem=${serverItem?.serviceNowKey}`);
          }
        }}
      />
    </Col>
  );
}
