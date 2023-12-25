import { IServerItemModel, ISortableCodeModel, ITenantModel } from '.';

export interface IOrganizationModel extends ISortableCodeModel<number> {
  parentId?: number;
  parent?: IOrganizationModel;

  // ServiceNow data
  rawData?: any;

  serviceNowKey: string;

  // Collections
  tenants?: ITenantModel[];
  children?: IOrganizationModel[];
  serverItems?: IServerItemModel[];
}
