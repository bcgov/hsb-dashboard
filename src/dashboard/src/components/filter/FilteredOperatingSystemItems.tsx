'use client';

import { FilterDropdown } from '@/components';
import { IServerItemListModel } from '@/hooks';
import { useFilteredOperatingSystemItems, useFilteredServerItems } from '@/hooks/filter';
import { useOperatingSystemItems, useServerItems } from '@/hooks/lists';
import { useFilteredStore } from '@/store';
import React from 'react';

export interface IFilteredOperatingSystemItemsProps {}

export const FilteredOperatingSystemItems = ({}: IFilteredOperatingSystemItemsProps) => {
  const { isReady: operatingSystemItemsReady, operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady, serverItems } = useServerItems();

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);
  const setLoading = useFilteredStore((state) => state.setLoading);

  const filteredOperatingSystemItems = useFilteredStore((state) => state.operatingSystemItems);
  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { options: filteredOperatingSystemItemOptions } = useFilteredOperatingSystemItems();

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { findServerItems } = useFilteredServerItems({
    useSimple: true,
  });

  React.useEffect(() => {
    if (!filteredOperatingSystemItems.length && !!operatingSystemItems.length)
      setFilteredOperatingSystemItems(operatingSystemItems);
    if (operatingSystemItems.length === 1)
      setValues((values) => ({ ...values, operatingSystemItem: operatingSystemItems[0] }));
  }, [
    setFilteredOperatingSystemItems,
    operatingSystemItems,
    setValues,
    filteredOperatingSystemItems.length,
  ]);

  return (
    <FilterDropdown
      label="Operating system"
      options={filteredOperatingSystemItemOptions}
      placeholder="Select OS"
      value={values.operatingSystemItem?.id ?? ''}
      disabled={!operatingSystemItemsReady || !serverItemsReady}
      loading={!operatingSystemItemsReady}
      onChange={async (value) => {
        const operatingSystemItem = operatingSystemItems.find((o) => o.id == value);
        setLoading(true);
        setValues((state) => ({ ...state, operatingSystemItem }));

        if (operatingSystemItem) {
          let filteredServerItems: IServerItemListModel[];
          if (serverItems.length) {
            filteredServerItems = serverItems.filter(
              (server) =>
                (values.tenant ? server.tenantId === values.tenant.id : true) &&
                (values.organization ? server.organizationId === values.organization.id : true) &&
                server.operatingSystemItemId === operatingSystemItem.id,
            );
            setFilteredServerItems(filteredServerItems ?? []);
          } else {
            filteredServerItems = await findServerItems({
              installStatus: 1,
              tenantId: values.tenant?.id,
              organizationId: values.organization?.id,
              operatingSystemItemId: operatingSystemItem.id,
            });
          }
          const serverItem = filteredServerItems?.length === 1 ? filteredServerItems[0] : undefined;

          setValues((state) => ({ ...state, operatingSystemItem, serverItem }));
        } else {
          if (serverItems.length) {
            const filteredServerItems = serverItems.filter(
              (server) =>
                (values.tenant ? server.tenantId === values.tenant.id : true) &&
                (values.organization ? server.organizationId === values.organization.id : true),
            );
            setFilteredServerItems(filteredServerItems ?? []);
          } else {
            await findServerItems({
              installStatus: 1,
              tenantId: values.tenant?.id,
              organizationId: values.organization?.id,
            });
          }
        }
        setLoading(false);
      }}
    />
  );
};
