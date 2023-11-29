'use client';

import { useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import React from 'react';

export default function Page() {
  const session = useSession();
  const router = useRouter();

  const isLoading = session?.status === 'loading';

  React.useEffect(() => {
    if (session?.status !== 'loading' && session?.status !== 'authenticated') router.push('/login');
  }, [router, session]);

  return (
    <main>
      <div>Welcome Home Page</div>
      {isLoading && 'Loading...'}
    </main>
  );
}
