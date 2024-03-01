import { useAdminStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IServerItemModel, useApiServerItems, useAuth } from '..';

export interface IAdminServerItemsProps {
  init?: boolean;
}

export const useAdminServerItems = ({ init }: IAdminServerItemsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiServerItems();
  const serverItems = useAdminStore((state) => state.serverItems);
  const setServerItems = useAdminStore((state) => state.setServerItems);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
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
    } else if (serverItems.length) setIsReady(true);
  }, [setServerItems, status, serverItems.length, find, isLoading, isReady, init]);

  return { isReady, serverItems };
};
