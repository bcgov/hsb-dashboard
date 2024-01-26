import { useApp } from '@/store';
import React from 'react';
import { IOrganizationModel, useApiOrganizations, useAuth } from '..';

export interface IOrganizationsProps {
  init?: boolean;
  includeTenants?: boolean;
}

export const useOrganizations = ({ init, includeTenants }: IOrganizationsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiOrganizations();
  const organizations = useApp((state) => state.organizations);
  const setOrganizations = useApp((state) => state.setOrganizations);

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
        .catch((error) => {
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
