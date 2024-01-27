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
import { useFiltered } from '@/store';
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

  const filteredTenant = useFiltered((state) => state.tenant);
  const setFilteredTenant = useFiltered((state) => state.setTenant);
  const setFilteredTenants = useFiltered((state) => state.setTenants);
  const { options: filteredTenantOptions } = useFilteredTenants();

  const filteredOrganization = useFiltered((state) => state.organization);
  const setFilteredOrganization = useFiltered((state) => state.setOrganization);
  const setFilteredOrganizations = useFiltered((state) => state.setOrganizations);
  const { findOrganizations } = useFilteredOrganizations();

  const filteredOperatingSystemItem = useFiltered((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const setFilteredOperatingSystemItems = useFiltered((state) => state.setOperatingSystemItems);
  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();

  const setFilteredServerItem = useFiltered((state) => state.setServerItem);

  const enableTenants = isHSB || tenants.length > 0;

  React.useEffect(() => {
    if (tenants.length) setFilteredTenants(tenants);
    if (tenants.length === 1) setFilteredTenant(tenants[0]);
  }, [setFilteredTenant, setFilteredTenants, tenants]);

  return (
    filteredTenantOptions.length && (
      <Select
        label="Tenant"
        variant="filter"
        options={filteredTenantOptions}
        placeholder="Select tenant"
        value={filteredTenant?.id ?? ''}
        disabled={!tenantsReady || !enableTenants || !serverItemsReady}
        loading={!tenantsReady}
        onChange={async (value) => {
          const tenant = tenants.find((t) => t.id == value);
          setFilteredTenant(tenant);
          setFilteredOrganization();
          setFilteredOperatingSystemItem();
          setFilteredServerItem();

          if (tenant) {
            const organizations = await findOrganizations({ tenantId: tenant.id });
            if (organizations.length === 1) setFilteredOrganization(organizations[0]);

            const operatingSystems = await findOperatingSystemItems({
              tenantId: tenant.id,
              organizationId: filteredOrganization?.id,
            });
            if (operatingSystems.length === 1) setFilteredOperatingSystemItem(operatingSystems[0]);

            const serverItems = await onChange?.(
              tenant,
              organizations.length === 1 ? organizations[0] : filteredOrganization,
              operatingSystems.length === 1 ? operatingSystems[0] : filteredOperatingSystemItem,
            );
            if (serverItems?.length === 1) setFilteredServerItem(serverItems[0]);
          } else {
            setFilteredOrganizations(organizations);
            setFilteredOperatingSystemItems(operatingSystemItems);

            await onChange?.(tenant);
          }
        }}
      />
    )
  );
};
