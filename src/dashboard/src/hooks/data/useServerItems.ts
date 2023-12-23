import { useApp } from '@/store';
import React from 'react';
import { IServerItemModel, useApiServerItems, useAuth } from '..';

export const useServerItems = () => {
  const { status } = useAuth();
  const { findServerItems } = useApiServerItems();
  const serverItems = useApp((state) => state.serverItems);
  const setServerItems = useApp((state) => state.setServerItems);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length) {
      findServerItems()
        .then(async (res) => {
          const serverItems: IServerItemModel[] = await res.json();
          setServerItems(serverItems);
        })
        .catch((error) => {
          console.error(error);
        });
    }
  }, [findServerItems, setServerItems, status, serverItems.length]);

  return serverItems;
};
