'use client';

import { Dashboard } from '@/components';
import { useSecureRoute } from '@/hooks';

export default function Page() {
  useSecureRoute((state) => state.isClient, '/');

  return <Dashboard />;
}
