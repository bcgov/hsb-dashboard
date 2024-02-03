import { IOption } from '@/components';
import { useFilteredStore } from '@/store';
import React from 'react';
import {
  IOperatingSystemItemFilter,
  IOperatingSystemItemModel,
  useApiOperatingSystemItems,
} from '..';

export const useFilteredOperatingSystemItems = () => {
  const { find } = useApiOperatingSystemItems();
  const { operatingSystemItem } = useFilteredStore((state) => state.values);
  const operatingSystemItems = useFilteredStore((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useFilteredStore((state) => state.setOperatingSystemItems);

  const [isLoading, setIsLoading] = React.useState(false);

  const fetch = React.useCallback(
    async (filter: IOperatingSystemItemFilter) => {
      try {
        setIsLoading(true);
        const res = await find(filter);
        const operatingSystemItems: IOperatingSystemItemModel[] = await res.json();
        setOperatingSystemItems(operatingSystemItems);
        return operatingSystemItems;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [find, setOperatingSystemItems],
  );

  const options = React.useMemo(
    () =>
      operatingSystemItems
        .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
        .map<IOption<IOperatingSystemItemModel>>((t) => ({
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
