import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IOrganizationModel, useApiOrganizations, useAuth } from '.';

export const useOrganizations = () => {
  const { status } = useAuth();
  const { findOrganizations } = useApiOrganizations();
  const organizations = useApp((state) => state.organizations);
  const setOrganizations = useApp((state) => state.setOrganizations);

  React.useEffect(() => {
    // Get an array of organizations.
    if (status === 'authenticated' && !organizations.length) {
      findOrganizations().then(async (res) => {
        const organizations: IOrganizationModel[] = await res.json();
        setOrganizations(organizations);
      });
    }
  }, [findOrganizations, setOrganizations, status, organizations.length]);

  const options = React.useMemo(
    () =>
      organizations.map<IOption<IOrganizationModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [organizations],
  );

  return { organizations, options };
};
