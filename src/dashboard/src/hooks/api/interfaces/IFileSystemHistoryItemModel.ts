import { IAuditableModel } from '.';

export interface IFileSystemHistoryItemModel extends IAuditableModel {
  id: number;

  // ServerNow data
  rawData?: any;
  rawDataCI?: any;

  serviceNowKey: string;
  serverItemServiceNowKey: string;

  className: string;
  name: string;
  label: string;
  category: string;
  subCategory: string;
  storageType: string;
  mediaType: string;
  volumeId: string;
  capacity: number;
  diskSpace: number;
  size: string;
  sizeBytes: number;
  usedSizeBytes?: number;
  availableSpace: number;
  freeSpace: string;
  freeSpaceBytes: number;
}
