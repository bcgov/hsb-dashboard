import { ISortableModel } from '.';

export interface ISortableCodeModel<T extends string | number> extends ISortableModel<T> {
  code: string;
}
