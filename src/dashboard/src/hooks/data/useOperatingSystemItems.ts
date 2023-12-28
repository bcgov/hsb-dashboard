import { useApp } from '@/store';
import React from 'react';
import { IOperatingSystemItemModel, useApiOperatingSystemItems, useAuth } from '..';

export const useOperatingSystemItems = () => {
  const { status } = useAuth();
  const { find } = useApiOperatingSystemItems();
  const operatingSystemItems = useApp((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useApp((state) => state.setOperatingSystemItems);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of operatingSystemItems.
    if (status === 'authenticated' && !operatingSystemItems.length) {
      setIsReady(false);
      find()
        .then(async (res) => {
          const operatingSystemItems: IOperatingSystemItemModel[] = await res.json();
          setOperatingSystemItems(operatingSystemItems);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (operatingSystemItems.length) setIsReady(true);
  }, [find, setOperatingSystemItems, status, operatingSystemItems.length]);

  return { isReady, operatingSystemItems };
};
