'use client';

import { Select } from '@/components';
import {
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks';
import { useOperatingSystemItems, useServerItems } from '@/hooks/data';
import { useFilteredOperatingSystemItems } from '@/hooks/filter';
import { useFilteredStore } from '@/store';
import React from 'react';

export interface IFilteredOperatingSystemItemsProps {
  /** Event fires when the selected tenant changes. */
  onChange?: (
    tenant?: ITenantModel,
    organization?: IOrganizationModel,
    operatingSystemItem?: IOperatingSystemItemModel,
  ) => Promise<IServerItemModel[]>;
}

export const FilteredOperatingSystemItems = ({ onChange }: IFilteredOperatingSystemItemsProps) => {
  const { isReady: operatingSystemItemsReady, operatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const { isReady: serverItemsReady } = useServerItems({
    useSimple: true,
  });

  const filteredTenant = useFilteredStore((state) => state.tenant);

  const filteredOrganization = useFilteredStore((state) => state.organization);

  const filteredOperatingSystemItem = useFilteredStore((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFilteredStore((state) => state.setOperatingSystemItem);
  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { options: filteredOperatingSystemItemOptions } = useFilteredOperatingSystemItems();

  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);

  React.useEffect(() => {
    if (operatingSystemItems.length) setFilteredOperatingSystemItems(operatingSystemItems);
    if (operatingSystemItems.length === 1) setFilteredOperatingSystemItem(operatingSystemItems[0]);
  }, [setFilteredOperatingSystemItems, operatingSystemItems, setFilteredOperatingSystemItem]);

  return (
    <Select
      label="Operating system"
      variant="filter"
      options={filteredOperatingSystemItemOptions}
      placeholder="Select OS"
      value={filteredOperatingSystemItem?.id ?? ''}
      disabled={!operatingSystemItemsReady || !serverItemsReady}
      loading={!operatingSystemItemsReady}
      onChange={async (value) => {
        const operatingSystemItem = operatingSystemItems.find((o) => o.id == value);
        setFilteredOperatingSystemItem(operatingSystemItem);
        setFilteredServerItem();

        if (operatingSystemItem) {
          const serverItems = await onChange?.(
            filteredTenant,
            filteredOrganization,
            operatingSystemItem,
          );
          if (serverItems?.length === 1) setFilteredServerItem(serverItems[0]);
        } else {
          await onChange?.(filteredTenant, filteredOrganization, operatingSystemItem);
        }
      }}
    />
  );
};
