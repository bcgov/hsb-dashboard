import { useAdminStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IOrganizationModel, useApiOrganizations, useAuth } from '..';

export interface IAdminOrganizationsProps {
  init?: boolean;
  includeTenants?: boolean;
}

export const useAdminOrganizations = ({ init, includeTenants }: IAdminOrganizationsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiOrganizations();
  const organizations = useAdminStore((state) => state.organizations);
  const setOrganizations = useAdminStore((state) => state.setOrganizations);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of organizations.
    if (status === 'authenticated' && !organizations.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      find({ includeTenants })
        .then(async (res) => {
          const organizations: IOrganizationModel[] = await res.json();
          setOrganizations(organizations);
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
    } else if (organizations.length) setIsReady(true);
  }, [
    find,
    setOrganizations,
    status,
    organizations.length,
    isLoading,
    isReady,
    init,
    includeTenants,
  ]);

  return { isReady, organizations };
};
