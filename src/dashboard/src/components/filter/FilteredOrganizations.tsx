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

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);

  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const { options: filteredOrganizationOptions } = useFilteredOrganizations();

  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);

  const enableOrganizations = isHSB || organizations.length > 0;

  React.useEffect(() => {
    if (organizations.length) setFilteredOrganizations(organizations);
    if (organizations.length === 1)
      setValues((values) => ({ ...values, organization: organizations[0] }));
  }, [setFilteredOrganizations, organizations, setValues]);

  return (
    <Select
      label="Organization"
      variant="filter"
      options={filteredOrganizationOptions}
      placeholder="Select organization"
      value={values.organization?.id ?? ''}
      disabled={!organizationsReady || !enableOrganizations || !serverItemsReady}
      loading={!organizationsReady}
      onChange={async (value) => {
        const organization = organizations.find((o) => o.id == value);
        setValues((state) => ({ ...state, organization }));

        if (organization) {
          const operatingSystems = await findOperatingSystemItems({
            tenantId: values.tenant?.id,
            organizationId: organization.id,
          });
          const operatingSystemItem =
            operatingSystems.length === 1 ? operatingSystems[0] : values.operatingSystemItem;

          const serverItems = await onChange?.(
            values.tenant,
            organization,
            operatingSystems.length === 1 ? operatingSystems[0] : values.operatingSystemItem,
          );
          const serverItem = serverItems?.length === 1 ? serverItems[0] : values.serverItem;

          setFilteredServerItems(serverItems ?? []);
          setValues((state) => ({ ...state, organization, operatingSystemItem, serverItem }));
        } else {
          setFilteredOperatingSystemItems(operatingSystemItems);
          const serverItems = await onChange?.(values.tenant, organization);
          setFilteredServerItems(serverItems ?? []);
        }
      }}
    />
  );
};
