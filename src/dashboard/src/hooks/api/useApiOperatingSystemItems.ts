import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IOperatingSystemItemFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiOperatingSystemItems = () => {
  return React.useMemo(
    () => ({
      find: async (filter: IOperatingSystemItemFilter | undefined = {}): Promise<Response> => {
        return await dispatch(`/api/dashboard/operating-system-items?${toQueryString(filter)}`);
      },
      findList: async (filter: IOperatingSystemItemFilter | undefined = {}): Promise<Response> => {
        return await dispatch(
          `/api/dashboard/operating-system-items/list?${toQueryString(filter)}`,
        );
      },
    }),
    [],
  );
};
