import { useFilteredStore } from '@/store';
import { getOrganizationOptions } from '@/utils';
import React from 'react';
import { IOrganizationFilter, IOrganizationModel, useApiOrganizations } from '..';

export interface IFilteredOrganizations {
  /** Whether to include disabled options */
  includeDisabled?: boolean;
}

export const useFilteredOrganizations = ({ includeDisabled }: IFilteredOrganizations = {}) => {
  const { find } = useApiOrganizations();
  const organizations = useFilteredStore((state) => state.organizations);
  const setOrganizations = useFilteredStore((state) => state.setOrganizations);

  const [isLoading, setIsLoading] = React.useState(false);

  const fetch = React.useCallback(
    async (filter: IOrganizationFilter) => {
      try {
        setIsLoading(true);
        const res = await find(filter);
        const organizations: IOrganizationModel[] = await res.json();
        setOrganizations(organizations);
        return organizations;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [find, setOrganizations],
  );

  const options = React.useMemo(
    () => getOrganizationOptions(organizations, includeDisabled),
    [organizations, includeDisabled],
  );

  return React.useMemo(
    () => ({ isLoading, findOrganizations: fetch, options, organizations }),
    [isLoading, organizations, fetch, options],
  );
};
