'use client';

import { keycloakSessionLogOut } from '@/utils';
import { signIn, useSession } from 'next-auth/react';

export const AuthState = () => {
  const session = useSession();

  if (session.status === 'loading') return <div>loading...</div>;
  else if (session.status === 'authenticated') {
    return (
      <div>
        <button onClick={() => keycloakSessionLogOut()}>Logout -</button>
      </div>
    );
  }

  return (
    <div>
      <button onClick={() => signIn('keycloak')}>Login +</button>
    </div>
  );
};
