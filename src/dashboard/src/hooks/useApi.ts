import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IUserFilter, IUserModel } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApi = () => {
  return React.useMemo(
    () => ({
      userinfo: async (): Promise<Response> => {
        const url = `/api/auth/userinfo`;
        const res = await dispatch(url, {
          method: 'POST',
        });
        if (!res.ok) throw new Error(`${res.status}: ${res.statusText}`);
        return res;
      },
      findUsers: async (filter: IUserFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/hsb/admin/users?${toQueryString(filter)}`);
      },
      updateUser: async (model: IUserModel): Promise<Response> => {
        const url = `/api/hsb/admin/users/${model.id}`;
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
