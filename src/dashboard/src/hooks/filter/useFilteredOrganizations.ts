import { useFiltered } from '@/store';
import { getOrganizationOptions } from '@/utils';
import React from 'react';
import { IOrganizationFilter, IOrganizationModel, useApiOrganizations } from '..';

export interface IFilteredOrganizations {
  /** Whether to include disabled options */
  includeDisabled?: boolean;
}

export const useFilteredOrganizations = ({ includeDisabled }: IFilteredOrganizations = {}) => {
  const { find } = useApiOrganizations();
  const organizations = useFiltered((state) => state.organizations);
  const setOrganizations = useFiltered((state) => state.setOrganizations);

  const fetch = React.useCallback(
    async (filter: IOrganizationFilter) => {
      const res = await find(filter);
      const organizations: IOrganizationModel[] = await res.json();
      setOrganizations(organizations);
      return organizations;
    },
    [find, setOrganizations],
  );

  const options = React.useMemo(
    () => getOrganizationOptions(organizations, includeDisabled),
    [organizations, includeDisabled],
  );

  return React.useMemo(
    () => ({
      findOrganizations: fetch,
      options,
      organizations,
    }),
    [organizations, fetch, options],
  );
};
