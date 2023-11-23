'use client';

import { signIn } from 'next-auth/react';

export default function Page() {
  return (
    <div>
      <button onClick={() => signIn('keycloak')}>Login +</button>
    </div>
  );
}
