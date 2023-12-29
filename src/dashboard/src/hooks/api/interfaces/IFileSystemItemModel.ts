import { IAuditableModel, IFileSystemHistoryItemModel, IServerItemModel } from '.';

export interface IFileSystemItemModel extends IAuditableModel {
  serviceNowKey: string;

  serverItemServiceNowKey: string;
  serverItem?: IServerItemModel;

  // ServerNow data
  rawData?: any;
  rawDataCI?: any;

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

  // Collections
  history?: IFileSystemHistoryItemModel[];
}
