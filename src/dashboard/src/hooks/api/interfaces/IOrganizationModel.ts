import { ISortableCodeModel } from '.';

export interface IOrganizationModel extends ISortableCodeModel<number> {
  serviceNowKey: string;
  rawData?: any;
}
