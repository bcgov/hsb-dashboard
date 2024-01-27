import { IUserModel } from '@/hooks';

export const searchUsers = (users: IUserModel[], search?: string) => {
  const value = search?.toLowerCase();
  return value
    ? users.filter(
        (r) =>
          r.username.toLowerCase().includes(value) ||
          r.displayName.toLowerCase().includes(value) ||
          r.email.toLowerCase().includes(value),
      )
    : users;
};
