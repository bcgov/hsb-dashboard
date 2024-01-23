import { useApp } from '@/store';
import React from 'react';
import { IServerItemModel, useApiServerItems, useAuth } from '..';

export interface IServerItemsProps {
  useSimple?: boolean;
  init?: boolean;
}

export const useServerItems = (
  { useSimple = false, init }: IServerItemsProps = { useSimple: false },
) => {
  const { status } = useAuth();
  const { find, findSimple } = useApiServerItems();
  const serverItems = useApp((state) => state.serverItems);
  const setServerItems = useApp((state) => state.setServerItems);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      if (useSimple) {
        findSimple()
          .then(async (res) => {
            const serverItems: IServerItemModel[] = await res.json();
            setServerItems(serverItems);
          })
          .catch((error) => {
            console.error(error);
          })
          .finally(() => {
            setIsReady(true);
            setIsLoading(false);
          });
      } else {
        find()
          .then(async (res) => {
            const serverItems: IServerItemModel[] = await res.json();
            setServerItems(serverItems);
          })
          .catch((error) => {
            console.error(error);
          })
          .finally(() => {
            setIsReady(true);
            setIsLoading(false);
          });
      }
    } else if (serverItems.length) setIsReady(true);
  }, [
    find,
    setServerItems,
    status,
    serverItems.length,
    useSimple,
    findSimple,
    isLoading,
    isReady,
    init,
  ]);

  return { isReady, serverItems };
};
