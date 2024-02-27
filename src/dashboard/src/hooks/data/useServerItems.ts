import { useAppStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
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
  const serverItems = useAppStore((state) => state.serverItems);
  const setServerItems = useAppStore((state) => state.setServerItems);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      if (useSimple) {
        findSimple({ installStatus: 1 })
          .then(async (res) => {
            const serverItems: IServerItemModel[] = await res.json();
            setServerItems(serverItems);
          })
          .catch((ex) => {
            const error = ex as Error;
            toast.error(error.message);
            console.error(error);
          })
          .finally(() => {
            setIsReady(true);
            setIsLoading(false);
          });
      } else {
        find({ installStatus: 1 })
          .then(async (res) => {
            const serverItems: IServerItemModel[] = await res.json();
            setServerItems(serverItems);
          })
          .catch((ex) => {
            const error = ex as Error;
            toast.error(error.message);
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
