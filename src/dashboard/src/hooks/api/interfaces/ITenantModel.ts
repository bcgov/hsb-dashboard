import { ISortableCodeModel } from '.';

export interface ITenantModel extends ISortableCodeModel<number> {
  serviceNowKey: string;
  rawData?: any;
}
