import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IConfigurationItemModel, useApiConfigurationItems, useAuth } from '.';

export const useConfigurationItems = () => {
  const { status } = useAuth();
  const { findConfigurationItems } = useApiConfigurationItems();
  const configurationItems = useApp((state) => state.configurationItems);
  const setConfigurationItems = useApp((state) => state.setConfigurationItems);

  React.useEffect(() => {
    // Get an array of configurationItems.
    if (status === 'authenticated' && !configurationItems.length) {
      findConfigurationItems().then(async (res) => {
        const configurationItems: IConfigurationItemModel[] = await res.json();
        setConfigurationItems(configurationItems);
      });
    }
  }, [findConfigurationItems, setConfigurationItems, status, configurationItems.length]);

  const options = React.useMemo(
    () =>
      configurationItems.map<IOption<IConfigurationItemModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [configurationItems],
  );

  return { configurationItems, options };
};
