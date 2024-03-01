import { IOption } from '@/components';
import { useAdminStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { IGroupModel, useAuth } from '..';
import { useApiGroups } from '../api/admin';

export interface IAdminGroupsProps {
  init?: boolean;
}

export const useAdminGroups = ({ init }: IAdminGroupsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiGroups();
  const groups = useAdminStore((state) => state.groups);
  const setGroups = useAdminStore((state) => state.setGroups);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of groups.
    if (status === 'authenticated' && !groups.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      find()
        .then(async (res) => {
          const groups: IGroupModel[] = await res.json();
          setGroups(groups);
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
    } else if (groups.length) setIsReady(true);
  }, [find, setGroups, status, groups.length, isLoading, isReady, init]);

  const options = React.useMemo(
    () =>
      groups
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
        .map<IOption<IGroupModel>>((t) => ({
          label: t.name,
          value: t.id,
          data: t,
          disabled: t.isEnabled,
        })),
    [groups],
  );

  return { isReady, groups, options };
};
