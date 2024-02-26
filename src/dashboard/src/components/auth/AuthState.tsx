'use client';

import { useApiUsers, useAuth } from '@/hooks';
import { useAppStore } from '@/store';
import { keycloakSessionLogOut } from '@/utils';
import _ from 'lodash';
import { signIn, useSession } from 'next-auth/react';
import { redirect } from 'next/navigation';
import React from 'react';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';
import { toast } from 'react-toastify';
import { Row } from '../flex';
import styles from './AuthState.module.scss';

export interface IAuthState {
  showName?: boolean;
}

export const AuthState: React.FC<IAuthState> = ({ showName }) => {
  const { userinfo: getUserinfo } = useApiUsers();
  const { status, session } = useAuth();
  const userinfo = useAppStore((state) => state.userinfo);
  const setUserinfo = useAppStore((state) => state.setUserinfo);
  const { update } = useSession();

  // Only ask for userinfo once.
  const [init, setInit] = React.useState(true);

  React.useEffect(() => {
    if (init && !userinfo && status === 'authenticated' && session) {
      setInit(false);
      getUserinfo()
        .then(async (res) => {
          const userinfo = await res.json();
          setUserinfo(userinfo);
          // The user activation process can automatically apply roles to a preapproved user.
          const roles = _.uniq(userinfo.groups.concat(session.user.roles));
          if (
            !session.user.roles.length ||
            !userinfo.groups.every((group: string) => roles.includes(group))
          ) {
            update({ ...session, user: { ...session.user, roles: roles } });
            redirect('/');
          }
        })
        .catch((error: any) => {
          toast.error('Failed to activate user.  Try to login again');
          console.error('Failed to activate user', error);
        });
    }
  }, [setUserinfo, status, getUserinfo, session, update, userinfo, init]);

  if (status === 'loading') return <div>loading...</div>;
  else if (status === 'authenticated') {
    return (
      <Row className="profile">
        {showName && <div>{session?.user.name}</div>}
        <button className={styles.button} onClick={() => keycloakSessionLogOut()} title="Sign out">
          <FaSignOutAlt />
          &nbsp;Sign out
        </button>
      </Row>
    );
  }

  return (
    <Row className="profile">
      <button className={styles.button} onClick={() => signIn('keycloak')} title="Sign in">
        <FaSignInAlt />
        &nbsp;Sign in
      </button>
    </Row>
  );
};
