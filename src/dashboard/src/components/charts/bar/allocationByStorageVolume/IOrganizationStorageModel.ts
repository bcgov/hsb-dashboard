import { IOrganizationListModel } from '@/hooks';

export interface IOrganizationStorageModel extends IOrganizationListModel {
  capacity: number;
  availableSpace: number;
  percentage: number;
}
