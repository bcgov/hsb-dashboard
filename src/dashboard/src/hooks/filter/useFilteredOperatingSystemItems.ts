import { IOption } from '@/components';
import { useFilteredStore } from '@/store';
import React from 'react';
import {
  IOperatingSystemItemFilter,
  IOperatingSystemItemListModel,
  useApiOperatingSystemItems,
} from '..';

export const useFilteredOperatingSystemItems = () => {
  const { findList } = useApiOperatingSystemItems();
  const { operatingSystemItem } = useFilteredStore((state) => state.values);
  const operatingSystemItems = useFilteredStore((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useFilteredStore((state) => state.setOperatingSystemItems);

  const [isLoading, setIsLoading] = React.useState(false);

  const fetch = React.useCallback(
    async (filter: IOperatingSystemItemFilter) => {
      try {
        setIsLoading(true);
        const res = await findList(filter);
        const operatingSystemItems: IOperatingSystemItemListModel[] = await res.json();
        setOperatingSystemItems(operatingSystemItems);
        return operatingSystemItems;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [findList, setOperatingSystemItems],
  );

  const options = React.useMemo(
    () =>
      operatingSystemItems
        .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
        .map<IOption<IOperatingSystemItemListModel>>((t) => ({
          label: t.name,
          value: t.id,
          data: t,
        })),
    [operatingSystemItems],
  );

  return React.useMemo(
    () => ({
      isLoading,
      findOperatingSystemItems: fetch,
      options,
      operatingSystemItems,
      operatingSystemItem,
    }),
    [isLoading, fetch, options, operatingSystemItems, operatingSystemItem],
  );
};
