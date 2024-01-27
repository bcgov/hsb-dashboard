import { IOption } from '@/components';
import { IUserModel } from '@/hooks';

export const getUserOptions = (users: IUserModel[]) => {
  return users
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
    }));
};
