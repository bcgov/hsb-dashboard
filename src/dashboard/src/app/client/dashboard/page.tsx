'use client';

import { Dashboard } from '@/components';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';
import { LoadingAnimation } from '@/components/charts/loadingAnimation';

export default function Page() {
  const state = useAuth();

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isClient) redirect('/');

  return <Dashboard />;
}
