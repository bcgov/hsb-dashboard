import { IServerItemListModel, ITenantListModel } from '.';
import { ISortableCodeModel } from '..';

export interface IOrganizationListModel extends ISortableCodeModel<number> {
  parentId?: number;
  parent?: IOrganizationListModel;

  serviceNowKey: string;

  // Collections
  tenants?: ITenantListModel[];
  children?: IOrganizationListModel[];
  serverItems?: IServerItemListModel[];
}
