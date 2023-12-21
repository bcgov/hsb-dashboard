import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IServerItemFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiServerItems = () => {
  return React.useMemo(
    () => ({
      findServerItems: async (filter: IServerItemFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/dashboard/server-items?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
