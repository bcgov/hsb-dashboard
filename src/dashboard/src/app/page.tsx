'use client';

import { LoadingAnimation } from '@/components/loadingAnimation';
import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

export default function Page() {
  const { isClient, isHSB, isAuthorized, isSystemAdmin, session } = useAuth();

  // Redirect to default page for each type of user.
  if (isClient) redirect('/client/dashboard');
  else if (isHSB) redirect('/hsb/dashboard');
  else if (isSystemAdmin) redirect('/hsb/admin/users');
  else if (isAuthorized) {
    // In this case, although authorized, the user does not have the roles we expect, so we have to
    // treat them as if they are unauthorized.
    redirect('/not-authorized');
  }

  return (
    <main>
      <LoadingAnimation />
    </main>
  );
}
