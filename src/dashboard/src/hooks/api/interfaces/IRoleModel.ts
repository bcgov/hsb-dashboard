import { IGroupModel, ISortableModel } from '.';

export interface IRoleModel extends ISortableModel<number> {
  key: string;
  groups?: IGroupModel[];
}
