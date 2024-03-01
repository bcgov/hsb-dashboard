import { IServerItemListModel } from '.';
import { IAuditableModel } from '..';

export interface IOperatingSystemItemListModel extends IAuditableModel {
  id: number;
  name: string;
  className: string;
  serviceNowKey: string;

  // Collections
  servers?: IServerItemListModel[];
}
