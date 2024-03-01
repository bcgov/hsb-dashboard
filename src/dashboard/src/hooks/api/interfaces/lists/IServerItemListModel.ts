import { IOperatingSystemItemListModel, IOrganizationListModel, ITenantListModel } from '.';
import { IAuditableModel } from '..';

export interface IServerItemListModel extends IAuditableModel {
  serviceNowKey: string;
  tenantId?: number;
  tenant?: ITenantListModel;
  organizationId?: number;
  organization?: IOrganizationListModel;
  operatingSystemItemId?: number;
  operatingSystemItem?: IOperatingSystemItemListModel;

  historyKey?: string;

  // ServiceNow data
  name: string;
  category: string;
  subCategory: string;
  dnsDomain: string;
  className: string;
  platform: string;
  ipAddress: string;
  fqdn: string;
  diskSpace?: string;

  capacity?: number;
  availableSpace?: number;
}
