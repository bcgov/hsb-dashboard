'use client';

import { AllocationTable } from '@/components';
import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import { useServerItems } from '@/hooks/data';
import { useDashboardStore, useFilteredStore } from '@/store';
import { redirect, useRouter } from 'next/navigation';

export default function Page() {
  const router = useRouter();
  const state = useAuth();
  const { isReady, serverItems } = useServerItems({ useSimple: true, init: true });
  const setValues = useFilteredStore((state) => state.setValues);
  const setDashboardServerItems = useDashboardStore((state) => state.setServerItems);

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isHSB) redirect('/');

  return (
    <AllocationTable
      margin={-75}
      serverItems={serverItems}
      loading={!isReady}
      onClick={(serverItem) => {
        if (serverItem) {
          setValues((state) => ({ serverItem }));
          setDashboardServerItems([serverItem]);
          router.push(`/hsb/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
    />
  );
}
