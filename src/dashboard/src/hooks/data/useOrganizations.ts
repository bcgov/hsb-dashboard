import { useApp } from '@/store';
import React from 'react';
import { IOrganizationModel, useApiOrganizations, useAuth } from '..';

export const useOrganizations = () => {
  const { status } = useAuth();
  const { find } = useApiOrganizations();
  const organizations = useApp((state) => state.organizations);
  const setOrganizations = useApp((state) => state.setOrganizations);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of organizations.
    if (status === 'authenticated' && !organizations.length) {
      setIsReady(false);
      find()
        .then(async (res) => {
          const organizations: IOrganizationModel[] = await res.json();
          setOrganizations(organizations);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (organizations.length) setIsReady(true);
  }, [find, setOrganizations, status, organizations.length]);

  return { isReady, organizations };
};
