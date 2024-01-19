'use client';

import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

export default function Page() {
  const state = useAuth();

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isSystemAdmin) redirect('/');

  return <div>HSB Admin</div>;
}
