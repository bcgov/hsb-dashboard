import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IOrganizationFilter, IOrganizationModel } from '../interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs organization out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiOrganizations = () => {
  return React.useMemo(
    () => ({
      find: async (filter: IOrganizationFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/admin/organizations?${toQueryString(filter)}`);
      },
      update: async (model: IOrganizationModel): Promise<Response> => {
        const url = `/api/admin/organizations/${model.id}`;
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
