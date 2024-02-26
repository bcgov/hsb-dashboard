import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { ITenantFilter, ITenantModel } from '../interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs tenant out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiTenants = () => {
  return React.useMemo(
    () => ({
      find: async (filter: ITenantFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/tenants?${toQueryString(filter)}`);
      },
      update: async (model: ITenantModel): Promise<Response> => {
        const url = `/api/admin/tenants/${model.id}`;
        const res = await dispatch(url, {
          method: 'PUT',
          headers: {
            'Content-type': 'application/json',
          },
          body: JSON.stringify(model),
        });
        return res;
      },
    }),
    [],
  );
};
