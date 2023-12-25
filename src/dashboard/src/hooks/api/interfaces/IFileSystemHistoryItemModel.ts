import { IAuditableModel } from '.';

export interface IFileSystemHistoryItemModel extends IAuditableModel {
  id: number;

  // ServerNow data
  rawData?: any;
  rawDataCI?: any;

  serviceNowKey: string;

  className: string;
  name: string;
  label: string;
  category: string;
  subCategory: string;
  storageType: string;
  mediaType: string;
  volumeId: string;
  capacity: string;
  diskSpace: string;
  size: string;
  sizeBytes: string;
  usedSizeBytes: string;
  availableSpace: string;
  freeSpace: string;
  freeSpaceBytes: string;
}
