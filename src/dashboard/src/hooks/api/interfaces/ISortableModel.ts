import { ICommonModel } from '.';

export interface ISortableModel<T extends string | number> extends ICommonModel<T> {
  sortOrder: number;
}
