'use client';

import { AllocationTable } from '@/components';
import { LoadingAnimation } from '@/components/charts/loadingAnimation';
import { useAuth } from '@/hooks';
import { useOperatingSystemItems, useServerItems } from '@/hooks/data';
import { useDashboardStore, useFilteredStore } from '@/store';
import { redirect, useRouter } from 'next/navigation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { isReady: isReadyOperatingSystemItems } = useOperatingSystemItems({ init: true });
  const { isReady: isReadyServerItems, serverItems } = useServerItems({
    useSimple: true,
    init: true,
  });
  const setValues = useFilteredStore((state) => state.setValues);
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
          setValues((state) => ({ serverItem }));
          setDashboardServerItems([serverItem]);
          router.push(`/client/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
    />
  );
}
