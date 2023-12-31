import { dispatch } from '@/utils';
import React from 'react';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiUsers = () => {
  return React.useMemo(
    () => ({
      userinfo: async (): Promise<Response> => {
        const url = `/api/auth/userinfo`;
        return await dispatch(url, {
          method: 'POST',
        });
      },
    }),
    [],
  );
};
