import { useSession } from 'next-auth/react';
import React from 'react';
import { RoleName } from './constants';

export const useAuth = () => {
  const { data: session, status } = useSession();

  return React.useMemo(
    () => ({
      isLoading: status === 'loading',
      isAuthenticated: status === 'authenticated',
      isAuthorized: session?.user.roles.length,
      isSystemAdmin: session?.user.roles.includes(RoleName.SystemAdmin),
      isHSB: session?.user.roles.includes(RoleName.HSB),
      isClient: session?.user.roles.includes(RoleName.Client),
      isOrganizationAdmin: session?.user.roles.includes(RoleName.OrganizationAdmin),
    }),
    [session?.user.roles, status],
  );
};
