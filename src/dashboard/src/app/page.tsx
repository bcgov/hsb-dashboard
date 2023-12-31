'use client';

import { useAuth } from '@/hooks';
import { redirect } from 'next/navigation';

export default function Page() {
  const { isClient, isHSB } = useAuth();

  // Redirect to default page for each type of user.
  if (isClient) redirect('/client/dashboard');
  else if (isHSB) redirect('/hsb/dashboard');

  return (
    <main>
      <div>Loading...</div>
    </main>
  );
}
