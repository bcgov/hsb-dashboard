import { useAppStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IOperatingSystemItemListModel, useApiOperatingSystemItems, useAuth } from '..';

export interface IOperatingSystemItemsProps {
  init?: boolean;
  includeOrganizations?: boolean;
  includeTenants?: boolean;
}

export const useOperatingSystemItems = ({
  init,
  includeOrganizations,
  includeTenants,
}: IOperatingSystemItemsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiOperatingSystemItems();
  const operatingSystemItems = useAppStore((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useAppStore((state) => state.setOperatingSystemItems);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of operatingSystemItems.
    if (
      status === 'authenticated' &&
      !operatingSystemItems.length &&
      !isLoading &&
      !isReady &&
      init
    ) {
      setIsLoading(true);
      setIsReady(false);
      find()
        .then(async (res) => {
          const operatingSystemItems: IOperatingSystemItemListModel[] = await res.json();
          setOperatingSystemItems(operatingSystemItems);
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
    } else if (operatingSystemItems.length) setIsReady(true);
  }, [
    find,
    setOperatingSystemItems,
    status,
    operatingSystemItems.length,
    isLoading,
    isReady,
    init,
    includeOrganizations,
    includeTenants,
  ]);

  return { isReady, operatingSystemItems };
};
