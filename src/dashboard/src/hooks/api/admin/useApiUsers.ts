import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IUserFilter, IUserModel } from '../interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiUsers = () => {
  return React.useMemo(
    () => ({
      find: async (filter: IUserFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/users?${toQueryString(filter)}`);
      },
      get: async (id: number): Promise<Response> => {
        return await dispatch(`/api/admin/users/${id}`);
      },
      update: async (model: IUserModel): Promise<Response> => {
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
    }),
    [],
  );
};
