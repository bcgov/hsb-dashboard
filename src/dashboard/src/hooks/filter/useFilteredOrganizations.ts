import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { IOrganizationFilter, IOrganizationModel, useApiOrganizations } from '..';

export const useFilteredOrganizations = () => {
  const { findOrganizations } = useApiOrganizations();
  const organizations = useFiltered((state) => state.organizations);
  const setOrganizations = useFiltered((state) => state.setOrganizations);

  const fetch = React.useCallback(
    async (filter: IOrganizationFilter) => {
      const res = await findOrganizations(filter);
      const organizations: IOrganizationModel[] = await res.json();
      setOrganizations(organizations);
      return organizations;
    },
    [findOrganizations, setOrganizations],
  );

  const options = React.useMemo(
    () =>
      organizations.map<IOption<IOrganizationModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [organizations],
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
