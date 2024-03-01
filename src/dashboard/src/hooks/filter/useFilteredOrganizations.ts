import { useFilteredStore } from '@/store';
import { getOrganizationOptions } from '@/utils';
import React from 'react';
import { IOrganizationFilter, IOrganizationListModel, useApiOrganizations } from '..';

export interface IFilteredOrganizations {
  /** Whether to include disabled options */
  includeDisabled?: boolean;
}

export const useFilteredOrganizations = ({ includeDisabled }: IFilteredOrganizations = {}) => {
  const { findList } = useApiOrganizations();
  const organizations = useFilteredStore((state) => state.organizations);
  const setOrganizations = useFilteredStore((state) => state.setOrganizations);

  const [isLoading, setIsLoading] = React.useState(false);

  const fetch = React.useCallback(
    async (filter: IOrganizationFilter) => {
      try {
        setIsLoading(true);
        const res = await findList(filter);
        const organizations: IOrganizationListModel[] = await res.json();
        setOrganizations(organizations);
        return organizations;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [findList, setOrganizations],
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
