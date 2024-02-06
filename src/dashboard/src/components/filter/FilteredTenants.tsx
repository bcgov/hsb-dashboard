'use client';

import { Select } from '@/components';
import {
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
  useAuth,
} from '@/hooks';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/data';
import {
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredTenants,
} from '@/hooks/filter';
import { useFilteredStore } from '@/store';
import React from 'react';

export interface IFilteredTenantsProps {
  /** Event fires when the selected tenant changes. */
  onChange?: (
    tenant?: ITenantModel,
    organization?: IOrganizationModel,
    operatingSystemItem?: IOperatingSystemItemModel,
  ) => Promise<IServerItemModel[]>;
}

export const FilteredTenants = ({ onChange }: IFilteredTenantsProps) => {
  const { isHSB } = useAuth();
  const { isReady: tenantsReady, tenants } = useTenants({ init: true });
  const { organizations } = useOrganizations();
  const { operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady } = useServerItems();

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);
  const setLoading = useFilteredStore((state) => state.setLoading);

  const setFilteredTenants = useFilteredStore((state) => state.setTenants);
  const { options: filteredTenantOptions } = useFilteredTenants();

  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const { findOrganizations } = useFilteredOrganizations();

  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);

  const enableTenants = isHSB || tenants.length > 0;

  React.useEffect(() => {
    if (tenants.length) setFilteredTenants(tenants);
    if (tenants.length === 1) setValues((values) => ({ ...values, tenant: tenants[0] }));
  }, [setFilteredTenants, setValues, tenants]);

  return filteredTenantOptions.length > 0 ? (
    <Select
      label="Tenant"
      variant="filter"
      options={filteredTenantOptions}
      placeholder="Select tenant"
      value={values.tenant?.id ?? ''}
      disabled={!tenantsReady || !enableTenants || !serverItemsReady}
      loading={!tenantsReady}
      onChange={async (value) => {
        const tenant = tenants.find((t) => t.id == value);
        setLoading(true);
        setValues((state) => ({ ...state, tenant }));

        if (tenant) {
          const organizations = await findOrganizations({ tenantId: tenant.id });
          const organization = organizations.length === 1 ? organizations[0] : undefined;

          const operatingSystems = await findOperatingSystemItems({
            tenantId: tenant.id,
            organizationId: organization?.id,
          });
          const operatingSystemItem =
            operatingSystems.length === 1 ? operatingSystems[0] : undefined;

          const serverItems = await onChange?.(
            tenant,
            organizations.length === 1 ? organizations[0] : organization,
            operatingSystems.length === 1 ? operatingSystems[0] : operatingSystemItem,
          );
          const serverItem = serverItems?.length === 1 ? serverItems[0] : undefined;

          setFilteredServerItems(serverItems ?? []);
          setValues((state) => ({ tenant, organization, operatingSystemItem, serverItem }));
        } else {
          setFilteredOrganizations(organizations);
          setFilteredOperatingSystemItems(operatingSystemItems);

          const serverItems = await onChange?.(tenant);
          setFilteredServerItems(serverItems ?? []);
        }
        setLoading(false);
      }}
    />
  ) : (
    <></>
  );
};
