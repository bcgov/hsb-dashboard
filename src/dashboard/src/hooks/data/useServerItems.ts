import { useApp } from '@/store';
import React from 'react';
import { IServerItemModel, useApiServerItems, useAuth } from '..';

export const useServerItems = () => {
  const { status } = useAuth();
  const { find } = useApiServerItems();
  const serverItems = useApp((state) => state.serverItems);
  const setServerItems = useApp((state) => state.setServerItems);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length) {
      setIsReady(false);
      find()
        .then(async (res) => {
          const serverItems: IServerItemModel[] = await res.json();
          setServerItems(serverItems);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (serverItems.length) setIsReady(true);
  }, [find, setServerItems, status, serverItems.length]);

  return { isReady, serverItems };
};
