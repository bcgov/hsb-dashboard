import { IOption } from '@/components';
import { useAdminStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IRoleModel, useAuth } from '..';
import { useApiRoles } from '../api/admin';

export interface IAdminRolesProps {
  init?: boolean;
}

export const useAdminRoles = ({ init }: IAdminRolesProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiRoles();
  const roles = useAdminStore((state) => state.roles);
  const setRoles = useAdminStore((state) => state.setRoles);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of roles.
    if (status === 'authenticated' && !roles.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      find()
        .then(async (res) => {
          const roles: IRoleModel[] = await res.json();
          setRoles(roles);
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
    } else if (roles.length) setIsReady(true);
  }, [find, setRoles, status, roles.length, isLoading, isReady, init]);

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
