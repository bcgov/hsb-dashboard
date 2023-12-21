'use client';

import { useApiUsers, useAuth } from '@/hooks';
import { useApp } from '@/store';
import { keycloakSessionLogOut } from '@/utils';
import { signIn } from 'next-auth/react';
import React from 'react';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';
import { Row } from '../flex';
import styles from './AuthState.module.scss';

export interface IAuthState {
  showName?: boolean;
}

export const AuthState: React.FC<IAuthState> = ({ showName }) => {
  const { userinfo } = useApiUsers();
  const { status, session } = useAuth();
  const setUserinfo = useApp((state) => state.setUserinfo);

  React.useEffect(() => {
    if (status === 'authenticated') {
      userinfo()
        .then(async (res) => {
          const userinfo = await res.json();
          setUserinfo(userinfo);
        })
        .catch((error: any) => {
          console.error('Failed to activate user', error);
        });
    }
  }, [userinfo, setUserinfo, status]);

  if (status === 'loading') return <div>loading...</div>;
  else if (status === 'authenticated') {
    return (
      <Row className="profile">
        {showName && <div>{session?.user.name}</div>}
          <button className={styles.button} onClick={() => keycloakSessionLogOut()} title="Sign out"><FaSignOutAlt />&nbsp;Sign out</button>
      </Row>
    );
  }

  return (
    <Row className="profile">
      <button className={styles.button} onClick={() => signIn('keycloak')} title="Sign in"><FaSignInAlt />&nbsp;Sign in</button>
    </Row>
  );
};
