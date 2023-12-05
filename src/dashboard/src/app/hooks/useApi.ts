import { toQueryString } from '@/utils';
import { signOut } from 'next-auth/react';
import React from 'react';
import { IUserFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApi = () => {
  return React.useMemo(
    () => ({
      findUsers: async (filter: IUserFilter | undefined = {}): Promise<Response> => {
        const response = await fetch(`/api/hsb/admin/users?${toQueryString(filter)}`);
        // If they are unauthenticated when making requests to the API then their token is expired.
        if (response.status === 401) signOut();
        return response;
      },
    }),
    [],
  );
};
