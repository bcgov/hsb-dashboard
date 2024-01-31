'use client';

import { Select } from '@/components';
import {
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
  useAuth,
} from '@/hooks';
import { useOperatingSystemItems, useOrganizations, useServerItems } from '@/hooks/data';
import { useFilteredOperatingSystemItems, useFilteredOrganizations } from '@/hooks/filter';
import { useFilteredStore } from '@/store';
import React from 'react';

export interface IFilteredOrganizationsProps {
  /** Event fires when the selected tenant changes. */
  onChange?: (
    tenant?: ITenantModel,
    organization?: IOrganizationModel,
    operatingSystemItem?: IOperatingSystemItemModel,
  ) => Promise<IServerItemModel[]>;
}

export const FilteredOrganizations = ({ onChange }: IFilteredOrganizationsProps) => {
  const { isHSB } = useAuth();
  const { isReady: organizationsReady, organizations } = useOrganizations({ init: true });
  const { operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady } = useServerItems();

  const filteredTenant = useFilteredStore((state) => state.tenant);

  const filteredOrganization = useFilteredStore((state) => state.organization);
  const setFilteredOrganization = useFilteredStore((state) => state.setOrganization);
  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const { options: filteredOrganizationOptions } = useFilteredOrganizations();

  const filteredOperatingSystemItem = useFilteredStore((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFilteredStore((state) => state.setOperatingSystemItem);
  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();

  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);

  const enableOrganizations = isHSB || organizations.length > 0;

  React.useEffect(() => {
    if (organizations.length) setFilteredOrganizations(organizations);
    if (organizations.length === 1) setFilteredOrganization(organizations[0]);
  }, [setFilteredOrganizations, organizations, setFilteredOrganization]);

  return (
    <Select
      label="Organization"
      variant="filter"
      options={filteredOrganizationOptions}
      placeholder="Select organization"
      value={filteredOrganization?.id ?? ''}
      disabled={!organizationsReady || !enableOrganizations || !serverItemsReady}
      loading={!organizationsReady}
      onChange={async (value) => {
        const organization = organizations.find((o) => o.id == value);
        setFilteredOrganization(organization);
        setFilteredOperatingSystemItem();
        setFilteredServerItem();

        if (organization) {
          const operatingSystems = await findOperatingSystemItems({
            tenantId: filteredTenant?.id,
            organizationId: organization.id,
          });
          if (operatingSystems.length === 1) setFilteredOperatingSystemItem(operatingSystems[0]);

          const serverItems = await onChange?.(
            filteredTenant,
            organization,
            operatingSystems.length === 1 ? operatingSystems[0] : filteredOperatingSystemItem,
          );
          if (serverItems?.length === 1) setFilteredServerItem(serverItems[0]);
        } else {
          setFilteredOperatingSystemItems(operatingSystemItems);
          await onChange?.(filteredTenant, organization);
        }
      }}
    />
  );
};
