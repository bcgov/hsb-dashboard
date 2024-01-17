'use client';

import { useSecureRoute } from '@/hooks';

export default function Page() {
  useSecureRoute((state) => state.isOrganizationAdmin, '/');

  return <div>Client User Admin</div>;
}
