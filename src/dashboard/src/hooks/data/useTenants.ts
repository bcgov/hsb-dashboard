import { useAppStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { ITenantModel, useApiTenants, useAuth } from '..';

export interface ITenantsProps {
  init?: boolean;
}

export const useTenants = ({ init }: ITenantsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiTenants();
  const tenants = useAppStore((state) => state.tenants);
  const setTenants = useAppStore((state) => state.setTenants);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of tenants.
    if (status === 'authenticated' && !tenants.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      find()
        .then(async (res) => {
          const tenants: ITenantModel[] = await res.json();
          setTenants(tenants);
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
    } else if (tenants.length) setIsReady(true);
  }, [find, init, isLoading, isReady, setTenants, status, tenants.length]);

  return { isReady, tenants };
};
