import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { ITenantFilter, ITenantModel, useApiTenants } from '..';

export const useFilteredTenants = () => {
  const { findTenants } = useApiTenants();
  const tenants = useFiltered((state) => state.tenants);
  const setTenants = useFiltered((state) => state.setTenants);

  const fetch = React.useCallback(
    async (filter: ITenantFilter) => {
      const res = await findTenants(filter);
      const tenants: ITenantModel[] = await res.json();
      setTenants(tenants);
      return tenants;
    },
    [findTenants, setTenants],
  );

  const options = React.useMemo(
    () =>
      tenants.map<IOption<ITenantModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [tenants],
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
