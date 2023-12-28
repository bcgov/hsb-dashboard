import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IUserModel, useAuth } from '..';
import { useApiUsers } from '../api/admin';

export const useUsers = () => {
  const { status } = useAuth();
  const { find } = useApiUsers();
  const users = useApp((state) => state.users);
  const setUsers = useApp((state) => state.setUsers);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of users.
    if (status === 'authenticated' && !users.length) {
      setIsReady(false);
      find()
        .then(async (res) => {
          const users: IUserModel[] = await res.json();
          setUsers(users);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (users.length) setIsReady(true);
  }, [find, setUsers, status, users.length]);

  const options = React.useMemo(
    () =>
      users
        .sort((a, b) =>
          a.username < a.username
            ? -1
            : a.username > b.username
            ? 1
            : a.email < b.email
            ? -1
            : a.email > b.email
            ? 1
            : 0,
        )
        .map<IOption<IUserModel>>((t) => ({
          label: t.username,
          value: t.id,
          data: t,
          disabled: t.isEnabled,
        })),
    [users],
  );

  return { isReady, users, options };
};
