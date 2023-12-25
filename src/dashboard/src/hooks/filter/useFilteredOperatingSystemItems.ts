import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import {
  IOperatingSystemItemFilter,
  IOperatingSystemItemModel,
  useApiOperatingSystemItems,
} from '..';

export const useFilteredOperatingSystemItems = () => {
  const { find } = useApiOperatingSystemItems();
  const operatingSystemItems = useFiltered((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useFiltered((state) => state.setOperatingSystemItems);

  const fetch = React.useCallback(
    async (filter: IOperatingSystemItemFilter) => {
      const res = await find(filter);
      const operatingSystemItems: IOperatingSystemItemModel[] = await res.json();
      setOperatingSystemItems(operatingSystemItems);
      return operatingSystemItems;
    },
    [find, setOperatingSystemItems],
  );

  const options = React.useMemo(
    () =>
      operatingSystemItems.map<IOption<IOperatingSystemItemModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [operatingSystemItems],
  );

  return React.useMemo(
    () => ({
      findOperatingSystemItems: fetch,
      options,
      operatingSystemItems,
    }),
    [operatingSystemItems, fetch, options],
  );
};
