import { Session } from 'next-auth';
import { useSession } from 'next-auth/react';
import React from 'react';
import { RoleName } from './constants';

export interface IAuthState {
  status: string;
  session: Session | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  isAuthorized: boolean;
  isSystemAdmin: boolean;
  isHSB: boolean;
  isClient: boolean;
  isOrganizationAdmin: boolean;
  roles: string[];
}

export const useAuth = () => {
  const { data: session, status } = useSession();

  return React.useMemo(() => {
    // This provides a way to manually override authentication/authorization.
    const roles = process.env.NEXT_PUBLIC_AUTH_ROLES?.split(',') ?? session?.user.roles ?? [];
    const oStatus = process.env.NEXT_PUBLIC_AUTH_STATUS ?? status;

    const state: IAuthState = {
      status: oStatus,
      session,
      isLoading: oStatus === 'loading',
      isAuthenticated: oStatus === 'authenticated',
      isAuthorized: !!roles.length,
      isSystemAdmin: roles.includes(RoleName.SystemAdmin),
      isHSB: roles.includes(RoleName.HSB),
      isClient: roles.includes(RoleName.Client),
      isOrganizationAdmin: roles.includes(RoleName.OrganizationAdmin),
      roles,
    };

    return state;
  }, [session, status]);
};
