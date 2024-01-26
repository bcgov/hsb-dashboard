import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IOrganizationFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiOrganizations = () => {
  return React.useMemo(
    () => ({
      find: async (filter: IOrganizationFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/dashboard/organizations?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
