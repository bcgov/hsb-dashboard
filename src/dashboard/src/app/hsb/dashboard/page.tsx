'use client';

import { Dashboard } from '@/components';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';
import { LoadingAnimation } from '@/components/charts/loadingAnimation';

export default function Page() {
  const state = useAuth();

  // Only allow HSB role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isHSB) redirect('/');

  return <Dashboard />;
}
