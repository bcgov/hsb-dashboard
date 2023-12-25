import { IAuditableModel, IServerItemModel } from '.';

export interface IOperatingSystemItemModel extends IAuditableModel {
  id: number;
  name: string;
  serviceNowKey: string;
  rawData?: any;

  // Collections
  servers?: IServerItemModel[];
}
