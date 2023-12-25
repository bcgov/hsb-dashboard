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
  capacity: string;
  diskSpace: string;
  size: string;
  sizeBytes: string;
  usedSizeBytes: string;
  availableSpace: string;
  freeSpace: string;
  freeSpaceBytes: string;

  // Collections
  history?: IFileSystemHistoryItemModel[];
}
