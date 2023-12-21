import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IConfigurationItemFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiConfigurationItems = () => {
  return React.useMemo(
    () => ({
      findConfigurationItems: async (
        filter: IConfigurationItemFilter | undefined = {},
      ): Promise<Response> => {
        return await dispatch(`/api/dashboard/configuration-items?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
