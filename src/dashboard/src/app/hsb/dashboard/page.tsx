'use client';

import { Dashboard, PageLoadingAnimation } from '@/components';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

export default function Page() {
  const state = useAuth();

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <PageLoadingAnimation />;
  if (!state.isHSB) redirect('/');

  return (
    <Dashboard />
  );
}
