import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IUserModel, useAuth } from '..';
import { useApiUsers } from '../api/admin';

interface IUserProps {
  includeGroups?: boolean;
}

export const useUsers = ({ includeGroups }: IUserProps) => {
  const { status } = useAuth();
  const { find, get } = useApiUsers();
  const userInfo = useApp((state) => state.userinfo);
  const users = useApp((state) => state.users);
  const setUsers = useApp((state) => state.setUsers);

  const [isReady, setIsReady] = React.useState(false);

  React.useEffect(() => {
    // Get an array of all users.
    if (status === 'authenticated' && !users.length) {
      setIsReady(false);
      find({ includeGroups })
        .then(async (res) => {
          const users: IUserModel[] = await res.json();
          setUsers(users);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => setIsReady(true));
    } else if (users.length) setIsReady(true);
  }, [find, includeGroups, setUsers, status, users.length]);

  React.useEffect(() => {
    // When a page is first loaded a request is made to activate the user.
    // This results in the user's information being updated after a request for the list of users.
    // Need to make a request for the latest user information.
    if (userInfo) {
      if (users.some((u) => u.id === userInfo.id && u.version !== userInfo.version)) {
        get(userInfo.id)
          .then(async (response) => {
            const user = await response.json();
            setUsers(users.map((u) => (u.id === userInfo.id ? user : u)));
          })
          .catch((error) => {
            console.error(error);
          });
      }
    }
  }, [userInfo, users, get, setUsers]);

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
