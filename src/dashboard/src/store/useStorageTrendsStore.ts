import { IFileSystemHistoryItemModel, IServerHistoryItemModel } from '@/hooks';
import { create } from 'zustand';

export interface IStorageTrendsStoreState {
  // Date Range for Total Storage
  dateRangeServerHistoryItems: string[];
  setDateRangeServerHistoryItems: (value?: string[]) => void;

  serverHistoryItemsReady?: boolean;
  setServerHistoryItemsReady: (value?: boolean) => void;
  serverHistoryItems: IServerHistoryItemModel[];
  setServerHistoryItems: (values: IServerHistoryItemModel[]) => void;

  // Date Range for each Volume
  dateRangeFileSystemHistoryItems: string[];
  setDateRangeFileSystemHistoryItems: (value?: string[]) => void;

  fileSystemHistoryItemsReady?: boolean;
  setFileSystemHistoryItemsReady: (value?: boolean) => void;
  fileSystemHistoryItems: IFileSystemHistoryItemModel[];
  setFileSystemHistoryItems: (values: IFileSystemHistoryItemModel[]) => void;
}

export const useStorageTrendsStore = create<IStorageTrendsStoreState>((set) => ({
  // Date Range for Total Storage
  dateRangeServerHistoryItems: [],
  setDateRangeServerHistoryItems: (values) =>
    set((state) => ({ dateRangeServerHistoryItems: values })),

  serverHistoryItemsReady: true,
  setServerHistoryItemsReady: (value) => set((state) => ({ serverHistoryItemsReady: value })),
  serverHistoryItems: [],
  setServerHistoryItems: (values) => set((state) => ({ serverHistoryItems: values })),

  // Date Range for each Volume
  dateRangeFileSystemHistoryItems: [],
  setDateRangeFileSystemHistoryItems: (values) =>
    set((state) => ({ dateRangeFileSystemHistoryItems: values })),

  fileSystemHistoryItemsReady: true,
  setFileSystemHistoryItemsReady: (value) =>
    set((state) => ({ fileSystemHistoryItemsReady: value })),
  fileSystemHistoryItems: [],
  setFileSystemHistoryItems: (values) => set((state) => ({ fileSystemHistoryItems: values })),
}));
