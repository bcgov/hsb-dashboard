import { IAuditableModel } from '.';

export interface IOperatingSystemItemModel extends IAuditableModel {
  id: number;
  name: string;
  serviceNowKey: string;
  rawData?: any;
}
