import { ISortableCodeModel } from '.';

export interface IOrganizationModel extends ISortableCodeModel<number> {
  parentId?: number;
  parent?: IOrganizationModel;

  children: IOrganizationModel[];

  rawData?: any;

  serviceNowKey: string;
}
