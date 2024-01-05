import { IOrganizationModel } from '@/hooks';

export interface IOrganizationStorageModel extends IOrganizationModel {
  capacity: number;
  availableSpace: number;
  percentage: number;
}
