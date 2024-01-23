'use client';

import { AllocationTable, Col } from '@/components';
import { useAuth } from '@/hooks';
import { useServerItems } from '@/hooks/data';
import { useDashboard, useFiltered } from '@/store';
import { redirect, useRouter } from 'next/navigation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { isReady, serverItems } = useServerItems({ useSimple: true, init: true });
  const setFilteredTenant = useFiltered((state) => state.setTenant);
  const setFilteredOrganization = useFiltered((state) => state.setOrganization);
  const setFilteredOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const setFilteredServerItem = useFiltered((state) => state.setServerItem);
  const setDashboardServerItems = useDashboard((state) => state.setServerItems);

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isHSB) redirect('/');

  return (
    <Col>
      <h1>All Servers</h1>
      <AllocationTable
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
    </Col>
  );
}
