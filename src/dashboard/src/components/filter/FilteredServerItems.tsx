'use client';

import { FilterDropdown } from '@/components';
import { useFilteredServerItems } from '@/hooks/filter';
import { useServerItems } from '@/hooks/lists';
import { useFilteredStore } from '@/store';
import React from 'react';

export const FilteredServerItems = () => {
  const { isReady: serverItemsReady, serverItems } = useServerItems();

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);
  const setLoading = useFilteredStore((state) => state.setLoading);

  const filteredServerItems = useFilteredStore((state) => state.serverItems);
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { options: filteredServerItemOptions } = useFilteredServerItems({
    useSimple: true,
  });

  React.useEffect(() => {
    if (!filteredServerItems.length && !!serverItems.length) setFilteredServerItems(serverItems);
    if (serverItems.length === 1)
      setValues((values) => ({ ...values, serverItem: serverItems[0] }));
  }, [setFilteredServerItems, serverItems, setValues, filteredServerItems.length]);

  return (
    <FilterDropdown
      label="Server"
      options={filteredServerItemOptions}
      placeholder="Select server"
      value={values.serverItem?.serviceNowKey ?? ''}
      disabled={!serverItemsReady}
      loading={!serverItemsReady}
      onChange={async (value) => {
        const serverItem = serverItems.find((o) => o.serviceNowKey == value);
        setLoading(true);
        setValues((values) => ({ ...values, serverItem }));
        setLoading(false);
      }}
    />
  );
};
