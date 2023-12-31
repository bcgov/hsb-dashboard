import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IRoleModel, useAuth } from '..';
import { useApiRoles } from '../api/admin';

export const useRoles = () => {
  const { status } = useAuth();
  const { find } = useApiRoles();
  const roles = useApp((state) => state.roles);
  const setRoles = useApp((state) => state.setRoles);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of roles.
    if (status === 'authenticated' && !roles.length) {
      setIsReady(false);
      find()
        .then(async (res) => {
          const roles: IRoleModel[] = await res.json();
          setRoles(roles);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (roles.length) setIsReady(true);
  }, [find, setRoles, status, roles.length]);

  const options = React.useMemo(
    () =>
      roles
        .sort((a, b) =>
          a.sortOrder < a.sortOrder
            ? -1
            : a.sortOrder > b.sortOrder
            ? 1
            : a.name < b.name
            ? -1
            : a.name > b.name
            ? 1
            : 0,
        )
        .map<IOption<IRoleModel>>((t) => ({
          label: t.name,
          value: t.id,
          data: t,
          disabled: t.isEnabled,
        })),
    [roles],
  );

  return { isReady, roles, options };
};
