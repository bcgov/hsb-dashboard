import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IGroupFilter, IGroupModel } from '../interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs group out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiGroups = () => {
  return React.useMemo(
    () => ({
      find: async (filter: IGroupFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/groups?${toQueryString(filter)}`);
      },
      update: async (model: IGroupModel): Promise<Response> => {
        const url = `/api/admin/groups/${model.id}`;
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
