import { useAdminStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IOperatingSystemItemModel, useApiOperatingSystemItems, useAuth } from '..';

export interface IAdminOperatingSystemItemsProps {
  init?: boolean;
  includeOrganizations?: boolean;
  includeTenants?: boolean;
}

export const useAdminOperatingSystemItems = ({
  init,
  includeOrganizations,
  includeTenants,
}: IAdminOperatingSystemItemsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiOperatingSystemItems();
  const operatingSystemItems = useAdminStore((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useAdminStore((state) => state.setOperatingSystemItems);

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
          const operatingSystemItems: IOperatingSystemItemModel[] = await res.json();
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
