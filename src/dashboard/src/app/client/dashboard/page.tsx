'use client';

import { Dashboard } from '@/components';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

export default function Page() {
  const state = useAuth();

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isClient) redirect('/');

  return <Dashboard />;
}
