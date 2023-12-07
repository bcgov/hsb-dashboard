'use client';

import { keycloakSessionLogOut } from '@/utils';
import { signIn, useSession } from 'next-auth/react';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';

export const AuthState = () => {
  const session = useSession();

  if (session.status === 'loading') return <div>loading...</div>;
  else if (session.status === 'authenticated') {
    return (
        <button onClick={() => keycloakSessionLogOut()} title="Sign out">
          <div className="group flex flex-row gap-1 items-center text-white-100 hover:text-gray-500 active:text-gray-900 font-bold">
            <FaSignOutAlt/>
            Sign out
          </div>
        </button>
    );
  }

  return (
      <button onClick={() => signIn('keycloak')} title="Sign in">
        <div className="group flex flex-row gap-1 items-center text-white-100 hover:text-gray-500 active:text-gray-900 font-bold">
          <FaSignInAlt/>
          Sign in
        </div>
      </button>
  );
};
