import { IAuditableModel, IConfigurationItemModel, IOperatingSystemItemModel } from '.';

export interface IServerItemModel extends IAuditableModel {
  id: number;
  configurationItemId?: number;
  configurationItem?: IConfigurationItemModel;
  operatingSystemItemId?: number;
  operatingSystemItem?: IOperatingSystemItemModel;

  rawData?: any;

  serviceNowKey: string;
  name: string;
  category: string;
  subCategory: string;
  diskSpace: string;
  dnsDomain: string;
  className: string;
  platform: string;
  iPAddress: string;
}
