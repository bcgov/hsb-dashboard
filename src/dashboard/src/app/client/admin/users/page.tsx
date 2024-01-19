'use client';

import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

export default function Page() {
  const state = useAuth();

  // Only allow Organization Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isOrganizationAdmin) redirect('/');

  return <div>Client User Admin</div>;
}
