import { dispatch, toQueryString } from '@/utils';
import React from 'react';
import { IFileSystemItemFilter } from './interfaces';

/**
 * Provides a simple way to manage all the API endpoints.
 * Signs user out if their token expires.
 * @returns API endpoint functions.
 */
export const useApiFileSystemItems = () => {
  return React.useMemo(
    () => ({
      findFileSystemItems: async (
        filter: IFileSystemItemFilter | undefined = {},
      ): Promise<Response> => {
        return await dispatch(`/api/dashboard/file-system-items?${toQueryString(filter)}`);
      },
    }),
    [],
  );
};
