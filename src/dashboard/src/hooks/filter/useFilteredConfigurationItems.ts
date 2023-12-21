import { useFiltered } from '@/store';
import React from 'react';
import { IConfigurationItemFilter, IConfigurationItemModel, useApiConfigurationItems } from '..';

export const useFilteredConfigurationItems = () => {
  const { findConfigurationItems } = useApiConfigurationItems();
  const configurationItems = useFiltered((state) => state.configurationItems);
  const setConfigurationItems = useFiltered((state) => state.setConfigurationItems);

  const fetch = React.useCallback(
    async (filter: IConfigurationItemFilter) => {
      const res = await findConfigurationItems(filter);
      const configurationItems: IConfigurationItemModel[] = await res.json();
      setConfigurationItems(configurationItems);
      return configurationItems;
    },
    [findConfigurationItems, setConfigurationItems],
  );

  return React.useMemo(
    () => ({
      findConfigurationItems: fetch,
      configurationItems,
    }),
    [configurationItems, fetch],
  );
};
