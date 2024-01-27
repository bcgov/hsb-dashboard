import { IUserModel, RoleName } from '@/hooks';
import { searchUsers as filter } from '@/utils';

export const searchUsers = (users: IUserModel[], search?: string) => {
  return filter(users, search).filter((user) => {
    return !user.groups?.some(
      (group) => group.name === RoleName.HSB || group.name === RoleName.SystemAdmin,
    );
  });
};
