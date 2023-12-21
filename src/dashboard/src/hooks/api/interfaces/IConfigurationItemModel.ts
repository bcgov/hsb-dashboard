import {
  IAuditableModel,
  IFileSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '.';

export interface IConfigurationItemModel extends IAuditableModel {
  id: number;
  tenantId?: number;
  tenant?: ITenantModel;
  organizationId?: number;
  organization?: IOrganizationModel;

  servers: IServerItemModel[];
  fileSystems: IFileSystemItemModel[];

  rawData?: any;

  serviceNowKey: string;
  name: string;
  category: string;
  subCategory: string;
  platform: string;
  dnsDomain: string;
  className: string;
  fQDN: string;
  iPAddress: string;
}
