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

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);
  const setLoading = useFilteredStore((state) => state.setLoading);

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { options: filteredServerItemOptions } = useFilteredServerItems({
    useSimple: true,
  });

  React.useEffect(() => {
    if (serverItems.length) setFilteredServerItems(serverItems);
    if (serverItems.length === 1)
      setValues((values) => ({ ...values, serverItem: serverItems[0] }));
  }, [setFilteredServerItems, serverItems, setValues]);

  return (
    <Select
      label="Server"
      variant="filter"
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
