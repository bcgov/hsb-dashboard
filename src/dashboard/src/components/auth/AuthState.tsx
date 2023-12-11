'use client';

import { useAuth } from '@/hooks';
import { keycloakSessionLogOut } from '@/utils';
import { signIn } from 'next-auth/react';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';
import { Row } from '../flex';

export interface IAuthState {
  showName?: boolean;
}

export const AuthState: React.FC<IAuthState> = ({ showName }) => {
  const { status, session } = useAuth();

  if (status === 'loading') return <div>loading...</div>;
  else if (status === 'authenticated') {
    return (
      <Row className="profile">
        {showName && <div>{session?.user.name}</div>}
        <button onClick={() => keycloakSessionLogOut()} title="Sign out">
          <div className="group flex flex-row gap-1 items-center text-white-100 hover:text-gray-500 active:text-gray-900 font-bold">
            <FaSignOutAlt />
            Sign out
          </div>
        </button>
      </Row>
    );
  }

  return (
    <button onClick={() => signIn('keycloak')} title="Sign in">
      <div className="group flex flex-row gap-1 items-center text-white-100 hover:text-gray-500 active:text-gray-900 font-bold">
        <FaSignInAlt />
        Sign in
      </div>
    </button>
  );
};
