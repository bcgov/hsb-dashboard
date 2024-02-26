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
      get: async (
        id: number,
        options: { includePermissions?: boolean } = {},
      ): Promise<Response> => {
        return await dispatch(`/api/admin/users/${id}?${toQueryString(options)}`);
      },
      add: async (model: IUserModel): Promise<Response> => {
        const url = `/api/admin/users`;
        const res = await dispatch(url, {
          method: 'POST',
          headers: {
            'Content-type': 'application/json',
          },
          body: JSON.stringify(model),
        });
        return res;
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
        return res;
      },
    }),
    [],
  );
};
