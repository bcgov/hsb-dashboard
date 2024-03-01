import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { ITenantFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiTenants = () => {
  return React.useMemo(
    () => ({
      find: async (filter: ITenantFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/dashboard/tenants?${toQueryString(filter)}`);
      },
      findList: async (filter: ITenantFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/dashboard/tenants/list?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
