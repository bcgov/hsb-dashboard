import { IOption } from '@/components';
import { useFilteredStore } from '@/store';
import React from 'react';
import { ITenantFilter, ITenantListModel, useApiTenants } from '..';

export interface IFilteredTenants {
  /** Whether to include disabled options */
  includeDisabled?: boolean;
}

export const useFilteredTenants = ({ includeDisabled }: IFilteredTenants = {}) => {
  const { findList } = useApiTenants();
  const isLoading = useFilteredStore((state) => state.loadingTenants);
  const setIsLoading = useFilteredStore((state) => state.setLoadingTenants);
  const tenants = useFilteredStore((state) => state.tenants);
  const setTenants = useFilteredStore((state) => state.setTenants);

  const fetch = React.useCallback(
    async (filter: ITenantFilter) => {
      try {
        setIsLoading(true);
        const res = await findList(filter);
        const tenants: ITenantListModel[] = await res.json();
        setTenants(tenants);
        return tenants;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [findList, setIsLoading, setTenants],
  );

  const options = React.useMemo(
    () =>
      tenants
        .filter((t) => (includeDisabled ? true : t.isEnabled))
        .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
        .map<IOption<ITenantListModel>>((t) => ({
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
