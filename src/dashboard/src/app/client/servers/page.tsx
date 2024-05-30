'use client';

import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';
import { Suspense } from 'react';
import ServerDashboard from './ServerDashboard';

export default function Page() {
  const state = useAuth();

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isClient) redirect('/');

  return (
    <Suspense fallback={<LoadingAnimation />}>
      <ServerDashboard />
    </Suspense>
  );
}
