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

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);

  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { options: filteredOperatingSystemItemOptions } = useFilteredOperatingSystemItems();

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);

  React.useEffect(() => {
    if (operatingSystemItems.length) setFilteredOperatingSystemItems(operatingSystemItems);
    if (operatingSystemItems.length === 1)
      setValues((values) => ({ ...values, operatingSystemItem: operatingSystemItems[0] }));
  }, [setFilteredOperatingSystemItems, operatingSystemItems, setValues]);

  return (
    <Select
      label="Operating system"
      variant="filter"
      options={filteredOperatingSystemItemOptions}
      placeholder="Select OS"
      value={values.operatingSystemItem?.id ?? ''}
      disabled={!operatingSystemItemsReady || !serverItemsReady}
      loading={!operatingSystemItemsReady}
      onChange={async (value) => {
        const operatingSystemItem = operatingSystemItems.find((o) => o.id == value);
        setValues((state) => ({ ...state, operatingSystemItem }));

        if (operatingSystemItem) {
          const serverItems = await onChange?.(
            values.tenant,
            values.organization,
            operatingSystemItem,
          );
          const serverItem = serverItems?.length === 1 ? serverItems[0] : values.serverItem;

          setFilteredServerItems(serverItems ?? []);
          setValues((state) => ({ ...state, operatingSystemItem, serverItem }));
        } else {
          const serverItems = await onChange?.(
            values.tenant,
            values.organization,
            operatingSystemItem,
          );
          setFilteredServerItems(serverItems ?? []);
        }
      }}
    />
  );
};
