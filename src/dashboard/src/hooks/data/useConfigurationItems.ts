import { useApp } from '@/store';
import React from 'react';
import { IConfigurationItemModel, useApiConfigurationItems, useAuth } from '..';

export const useConfigurationItems = () => {
  const { status } = useAuth();
  const { findConfigurationItems } = useApiConfigurationItems();
  const configurationItems = useApp((state) => state.configurationItems);
  const setConfigurationItems = useApp((state) => state.setConfigurationItems);

  React.useEffect(() => {
    if (status === 'authenticated' && !configurationItems.length) {
      // Get an array of configurationItems.
      findConfigurationItems()
        .then(async (res) => {
          const configurationItems: IConfigurationItemModel[] = await res.json();
          setConfigurationItems(configurationItems);
        })
        .catch((error) => {
          console.error(error);
        });
    }
  }, [status, configurationItems.length, findConfigurationItems, setConfigurationItems]);

  return configurationItems;
};
