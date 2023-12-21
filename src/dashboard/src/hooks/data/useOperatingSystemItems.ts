import { useApp } from '@/store';
import React from 'react';
import { IOperatingSystemItemModel, useApiOperatingSystemItems, useAuth } from '..';

export const useOperatingSystemItems = () => {
  const { status } = useAuth();
  const { findOperatingSystemItems } = useApiOperatingSystemItems();
  const operatingSystemItems = useApp((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useApp((state) => state.setOperatingSystemItems);

  React.useEffect(() => {
    // Get an array of operatingSystemItems.
    if (status === 'authenticated' && !operatingSystemItems.length) {
      findOperatingSystemItems()
        .then(async (res) => {
          const operatingSystemItems: IOperatingSystemItemModel[] = await res.json();
          setOperatingSystemItems(operatingSystemItems);
        })
        .catch((error) => {
          console.error(error);
        });
    }
  }, [findOperatingSystemItems, setOperatingSystemItems, status, operatingSystemItems.length]);

  return operatingSystemItems;
};
