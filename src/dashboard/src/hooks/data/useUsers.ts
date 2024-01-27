import { useApp } from '@/store';
import { getUserOptions } from '@/utils';
import React from 'react';
import { IUserModel, useAuth } from '..';
import { useApiUsers } from '../api/admin';

interface IUserProps {
  includePermissions?: boolean;
  init?: boolean;
}

export const useUsers = ({ includePermissions, init }: IUserProps = {}) => {
  const { status } = useAuth();
  const { find, get } = useApiUsers();
  const userInfo = useApp((state) => state.userinfo);
  const users = useApp((state) => state.users);
  const setUsers = useApp((state) => state.setUsers);

  const [isReady, setIsReady] = React.useState(false);
  const [isLoading, setIsLoading] = React.useState(false);

  React.useEffect(() => {
    // Get an array of all users.
    if (status === 'authenticated' && !users.length && !isLoading && !isReady && init) {
      setIsLoading(true);
      setIsReady(false);
      find({ includePermissions })
        .then(async (res) => {
          const users: IUserModel[] = await res.json();
          setUsers(users);
        })
        .catch((error) => {
          console.error(error);
        })
        .finally(() => {
          setIsReady(true);
          setIsLoading(false);
        });
    } else if (users.length) setIsReady(true);
  }, [find, includePermissions, init, isLoading, isReady, setUsers, status, users.length]);

  React.useEffect(() => {
    // When a page is first loaded a request is made to activate the user.
    // This results in the user's information being updated after a request for the list of users.
    // Need to make a request for the latest user information.
    if (userInfo) {
      if (users.some((u) => u.id === userInfo.id && u.version !== userInfo.version)) {
        get(userInfo.id, { includePermissions })
          .then(async (response) => {
            const user = await response.json();
            setUsers(users.map((u) => (u.id === userInfo.id ? user : u)));
          })
          .catch((error) => {
            console.error(error);
          });
      }
    }
  }, [userInfo, users, get, setUsers, includePermissions]);

  const options = React.useMemo(() => getUserOptions(users), [users]);

  return { isReady, users, options };
};
