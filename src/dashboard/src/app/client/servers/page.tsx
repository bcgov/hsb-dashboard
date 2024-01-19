'use client';

import { AllocationTable, Col } from '@/components';
import { useAuth } from '@/hooks';
import { useServerItems } from '@/hooks/data';
import { redirect } from 'next/navigation';

export default function Page() {
  const state = useAuth();
  const { isReady, serverItems } = useServerItems();

  // Only allow Client role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isClient) redirect('/');

  return (
    <Col>
      <h1>All Servers</h1>
      <AllocationTable serverItems={serverItems} loading={!isReady} />
    </Col>
  );
}
