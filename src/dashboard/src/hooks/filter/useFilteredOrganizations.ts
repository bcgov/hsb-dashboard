import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { IOrganizationFilter, IOrganizationModel, useApiOrganizations } from '..';

export const useFilteredOrganizations = () => {
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
    () =>
      organizations.map<IOption<IOrganizationModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
        disabled: t.isEnabled,
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
