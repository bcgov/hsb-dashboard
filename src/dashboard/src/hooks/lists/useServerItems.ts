import { useAppStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IServerItemListModel, useApiServerItems, useAuth } from '..';

export interface IServerItemsProps {
  init?: boolean;
}

export const useServerItems = ({ init }: IServerItemsProps = {}) => {
  const { status } = useAuth();
  const { findList } = useApiServerItems();
  const serverItems = useAppStore((state) => state.serverItems);
  const setServerItems = useAppStore((state) => state.setServerItems);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      findList({ installStatus: 1 })
        .then(async (res) => {
          const serverItems: IServerItemListModel[] = await res.json();
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
    } else if (serverItems.length) setIsReady(true);
  }, [setServerItems, status, serverItems.length, findList, isLoading, isReady, init]);

  return { isReady, serverItems };
};
