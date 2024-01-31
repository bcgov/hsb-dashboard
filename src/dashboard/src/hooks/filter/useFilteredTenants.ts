import { IOption } from '@/components';
import { useFilteredStore } from '@/store';
import React from 'react';
import { ITenantFilter, ITenantModel, useApiTenants } from '..';

export interface IFilteredTenants {
  /** Whether to include disabled options */
  includeDisabled?: boolean;
}

export const useFilteredTenants = ({ includeDisabled }: IFilteredTenants = {}) => {
  const { find } = useApiTenants();
  const tenants = useFilteredStore((state) => state.tenants);
  const setTenants = useFilteredStore((state) => state.setTenants);

  const [isLoading, setIsLoading] = React.useState(false);

  const fetch = React.useCallback(
    async (filter: ITenantFilter) => {
      try {
        setIsLoading(true);
        const res = await find(filter);
        const tenants: ITenantModel[] = await res.json();
        setTenants(tenants);
        return tenants;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
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
    () => ({ isLoading, findTenants: fetch, options, tenants }),
    [isLoading, tenants, fetch, options],
  );
};
