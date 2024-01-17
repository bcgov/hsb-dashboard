'use client';

import { useSecureRoute } from '@/hooks';

export default function Page() {
  useSecureRoute((state) => state.isSystemAdmin, '/');

  return <div>HSB Organization Admin</div>;
}
