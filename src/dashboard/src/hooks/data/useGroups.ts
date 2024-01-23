import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IGroupModel, useAuth } from '..';
import { useApiGroups } from '../api/admin';

export interface IGroupsProps {
  init?: boolean;
}

export const useGroups = ({ init }: IGroupsProps = {}) => {
  const { status } = useAuth();
  const { find } = useApiGroups();
  const groups = useApp((state) => state.groups);
  const setGroups = useApp((state) => state.setGroups);

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
        .catch((error) => {
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
