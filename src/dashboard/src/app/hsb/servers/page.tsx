'use client';

import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';
import { Suspense } from 'react';
import ServerDashboard from './ServerDashboard';

export default function Page() {
  const state = useAuth();

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isHSB) redirect('/');

  return (
    <Suspense fallback={<LoadingAnimation />}>
      <ServerDashboard />
    </Suspense>
  );
}
