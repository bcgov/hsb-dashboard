import {
  IAuditableModel,
  IFileSystemItemModel,
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerHistoryItemModel,
  ITenantModel,
} from '.';

export interface IServerItemModel extends IAuditableModel {
  serviceNowKey: string;
  tenantId?: number;
  tenant?: ITenantModel;
  organizationId?: number;
  organization?: IOrganizationModel;
  operatingSystemItemId?: number;
  operatingSystemItem?: IOperatingSystemItemModel;

  // ServiceNow data
  rawData?: any;
  rawDataCI?: any;

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

  // Collections
  fileSystemItems: IFileSystemItemModel[];
  history?: IServerHistoryItemModel[];
}
