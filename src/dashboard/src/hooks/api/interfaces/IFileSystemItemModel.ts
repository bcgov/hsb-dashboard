import { IAuditableModel, IConfigurationItemModel } from '.';

export interface IFileSystemItemModel extends IAuditableModel {
  id: number;
  configurationItemId: number;
  configurationItem?: IConfigurationItemModel;

  rawData?: any;

  serviceNowKey: string;
  name: string;
  label: string;
  category: string;
  subCategory: string;
  storageType: string;
  mediaType: string;
  volumeId: string;
  className: string;
  capacity: string;
  diskSpace: string;
  size: string;
  sizeBytes: string;
  usedSizeBytes: string;
  availableSpace: string;
  freeSpace: string;
  freeSpaceBytes: string;
}
