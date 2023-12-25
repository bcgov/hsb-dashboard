import { IOrganizationModel, IServerItemModel, ISortableCodeModel } from '.';

export interface ITenantModel extends ISortableCodeModel<number> {
  // ServiceNow data
  rawData?: any;
  serviceNowKey: string;

  // Collections
  organizations?: IOrganizationModel[];
  serverItems?: IServerItemModel[];
}
