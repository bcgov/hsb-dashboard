import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { ITenantFilter, IUserFilter, IUserModel } from '../interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiUsers = () => {
  return React.useMemo(
    () => ({
      findUsers: async (filter: IUserFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/users?${toQueryString(filter)}`);
      },
      updateUser: async (model: IUserModel): Promise<Response> => {
        const url = `/api/admin/users/${model.id}`;
        const res = await dispatch(url, {
          method: 'PUT',
          headers: {
            'Content-type': 'application/json',
          },
          body: JSON.stringify(model),
        });
        if (!res.ok) throw new Error(`${res.status}: ${res.statusText}`);
        return res;
      },
      findTenants: async (filter: ITenantFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/tenants?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
