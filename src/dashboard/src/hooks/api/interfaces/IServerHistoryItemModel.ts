import { IAuditableModel, IOperatingSystemItemModel, IOrganizationModel, ITenantModel } from '.';

export interface IServerHistoryItemModel extends IAuditableModel {
  id: number;
  tenantId?: number;
  tenant?: ITenantModel;
  organizationId?: number;
  organization?: IOrganizationModel;
  operatingSystemItemId?: number;
  operatingSystemItem?: IOperatingSystemItemModel;

  historyKey?: string;

  // ServiceNow data
  rawData?: any;
  rawDataCI?: any;

  serviceNowKey: string;
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
