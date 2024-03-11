'use client';

import { useApiUsers, useAuth } from '@/hooks';
import { IUserInfoModel } from '@/hooks/api/interfaces/auth';
import { useAdminStore, useAppStore } from '@/store';
import { keycloakSessionLogOut } from '@/utils';
import _ from 'lodash';
import { signIn, useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation';
import React from 'react';
import { FaSignInAlt, FaSignOutAlt } from 'react-icons/fa';
import { toast } from 'react-toastify';
import { Row } from '../flex';
import styles from './AuthState.module.scss';

export interface IAuthState {
  showName?: boolean;
}

/**
 * Activator class provides a thread-safe singleton.
 */
class Activator {
  private activating: boolean = false;

  constructor() {}

  /**
   * Only the first request to start will return true.
   * @returns Whether to start activating.
   */
  start() {
    if (!this.activating) {
      this.activating = true;
      return true;
    }
    return false;
  }

  getState() {
    return this.activating;
  }

  setState(state: boolean) {
    this.activating = state;
    return this.activating;
  }
}

let activator = new Activator();

export const AuthState: React.FC<IAuthState> = ({ showName }) => {
  const { userinfo: getUserinfo } = useApiUsers();
  const { status, session } = useAuth();
  const userinfo = useAppStore((state) => state.userinfo);
  const setUserinfo = useAppStore((state) => state.setUserinfo);
  const { update } = useSession();
  const router = useRouter();
  const adminUsers = useAdminStore((state) => state.users);
  const setAdminUsers = useAdminStore((state) => state.setUsers);

  React.useEffect(() => {
    if (!userinfo && status === 'authenticated' && session) {
      if (activator.start()) {
        if (session.user.roles.length === 0) toast.info('Attempting to preapprove your account');
        getUserinfo()
          .then(async (res) => {
            const userinfo: IUserInfoModel = await res.json();
            // The user activation process can automatically apply roles to a preapproved user.
            const roles = _.uniq(userinfo.groups.concat(session.user.roles));
            if (
              !session.user.roles.length ||
              !userinfo.groups.every((group: string) => roles.includes(group))
            ) {
              // Inform the session of the pre-authorized user role changes.
              await update({ ...session, user: { ...session.user, roles: roles } });
              setUserinfo(userinfo);
              router.push('/');
            } else {
              setUserinfo(userinfo);
            }
            setAdminUsers(
              adminUsers.map((user) =>
                user.id === userinfo.id
                  ? { ...user, version: userinfo.version ?? user.version }
                  : user,
              ),
            );
          })
          .catch((error: any) => {
            toast.error('Failed to activate user.  Try to login again');
            console.error('Failed to activate user', error);
            activator.setState(false);
          });
      }
    }
  }, [
    setUserinfo,
    status,
    getUserinfo,
    session,
    update,
    userinfo,
    router,
    setAdminUsers,
    adminUsers,
  ]);

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
