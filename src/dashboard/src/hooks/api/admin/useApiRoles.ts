import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IRoleFilter } from '../interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs role out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiRoles = () => {
  return React.useMemo(
    () => ({
      find: async (filter: IRoleFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/roles?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
