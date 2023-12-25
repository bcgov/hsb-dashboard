import { IRoleModel, ISortableModel, IUserModel } from '.';

export interface IGroupModel extends ISortableModel<number> {
  key: string;
  users?: IUserModel[];
  roles?: IRoleModel[];
}
