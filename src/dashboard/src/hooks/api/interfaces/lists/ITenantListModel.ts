import { IOrganizationListModel, IServerItemListModel } from '.';
import { ISortableCodeModel } from '..';

export interface ITenantListModel extends ISortableCodeModel<number> {
  // ServiceNow data
  serviceNowKey: string;

  // Collections
  organizations?: IOrganizationListModel[];
  serverItems?: IServerItemListModel[];
}
