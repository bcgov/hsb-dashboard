import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { ITenantFilter, ITenantModel, useApiTenants } from '..';

export interface IFilteredTenants {
  /** Whether to include disabled options */
  includeDisabled?: boolean;
}

export const useFilteredTenants = ({ includeDisabled }: IFilteredTenants = {}) => {
  const { find } = useApiTenants();
  const tenants = useFiltered((state) => state.tenants);
  const setTenants = useFiltered((state) => state.setTenants);

  const fetch = React.useCallback(
    async (filter: ITenantFilter) => {
      const res = await find(filter);
      const tenants: ITenantModel[] = await res.json();
      setTenants(tenants);
      return tenants;
    },
    [find, setTenants],
  );

  const options = React.useMemo(
    () =>
      tenants
        .filter((t) => (includeDisabled ? true : t.isEnabled))
        .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
        .map<IOption<ITenantModel>>((t) => ({
          label: t.name,
          value: t.id,
          data: t,
          disabled: t.isEnabled,
        })),
    [includeDisabled, tenants],
  );

  return React.useMemo(
    () => ({
      findTenants: fetch,
      options,
      tenants,
    }),
    [tenants, fetch, options],
  );
};
