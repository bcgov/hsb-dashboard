'use client';

import { keycloakSessionLogOut } from '@/utils';
import { signIn, useSession } from 'next-auth/react';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';

export const AuthState = () => {
  const session = useSession();

  if (session.status === 'loading') return <div>loading...</div>;
  else if (session.status === 'authenticated') {
    return (
      <div>
        <button onClick={() => keycloakSessionLogOut()} title="Sign out">
          <div className="group flex flex-row gap-1 items-center text-gray-100 hover:text-gray-500 active:text-gray-900">
            <FaSignOutAlt className="fill-gray-100 group-hover:fill-gray-500 group-active:fill-gray-900" />
            Sign out
          </div>
        </button>
      </div>
    );
  }

  return (
    <div>
      <button onClick={() => signIn('keycloak')} title="Sign in">
        <div className="group flex flex-row gap-1 items-center text-gray-100 hover:text-gray-500 active:text-gray-900">
          <FaSignInAlt className="fill-gray-100 hover:fill-gray-500 active:fill-gray-900" />
          Sign in
        </div>
      </button>
    </div>
  );
};
