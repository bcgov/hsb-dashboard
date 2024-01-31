'use client';

import { Select } from '@/components';
import { useServerItems } from '@/hooks/data';
import { useFilteredServerItems } from '@/hooks/filter';
import { useFilteredStore } from '@/store';
import React from 'react';

export const FilteredServerItems = () => {
  const { isReady: serverItemsReady, serverItems } = useServerItems({
    useSimple: true,
    init: true,
  });

  const filteredServerItem = useFilteredStore((state) => state.serverItem);
  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { options: filteredServerItemOptions } = useFilteredServerItems({
    useSimple: true,
  });

  React.useEffect(() => {
    if (serverItems.length) setFilteredServerItems(serverItems);
    if (serverItems.length === 1) setFilteredServerItem(serverItems[0]);
  }, [setFilteredServerItems, serverItems, setFilteredServerItem]);

  return (
    <Select
      label="Server"
      variant="filter"
      options={filteredServerItemOptions}
      placeholder="Select server"
      value={filteredServerItem?.serviceNowKey ?? ''}
      disabled={!serverItemsReady}
      loading={!serverItemsReady}
      onChange={async (value) => {
        const server = serverItems.find((o) => o.serviceNowKey == value);
        setFilteredServerItem(server);
      }}
    />
  );
};
