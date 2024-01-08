import {
  IFileSystemHistoryItemModel,
  IFileSystemItemModel,
  IOrganizationModel,
  IServerHistoryItemModel,
  IServerItemModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IDashboardState {
  // Date Range
  dateRange: string[];
  setDateRange: (value?: string[]) => void;

  // Organizations
  organizations: IOrganizationModel[];
  setOrganizations: (values: IOrganizationModel[]) => void;

  // Server Items
  serverItemsReady?: boolean;
  setServerItemsReady: (value?: boolean) => void;
  serverItems: IServerItemModel[];
  setServerItems: (values: IServerItemModel[]) => void;

  serverHistoryItemsReady?: boolean;
  setServerHistoryItemsReady: (value?: boolean) => void;
  serverHistoryItems: IServerHistoryItemModel[];
  setServerHistoryItems: (values: IServerHistoryItemModel[]) => void;

  // File System Items
  fileSystemItemsReady?: boolean;
  setFileSystemItemsReady: (value?: boolean) => void;
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;

  fileSystemHistoryItemsReady?: boolean;
  setFileSystemHistoryItemsReady: (value?: boolean) => void;
  fileSystemHistoryItems: IFileSystemHistoryItemModel[];
  setFileSystemHistoryItems: (values: IFileSystemHistoryItemModel[]) => void;
}

export const useDashboard = create<IDashboardState>((set) => ({
  // Date Range
  dateRange: [],
  setDateRange: (values) => set((state) => ({ dateRange: values })),

  // Organizations
  organizations: [],
  setOrganizations: (values) => set((state) => ({ organizations: values })),

  // Server Items
  serverItemsReady: true,
  setServerItemsReady: (value) => set((state) => ({ serverItemsReady: value })),
  serverItems: [],
  setServerItems: (values) => set((state) => ({ serverItems: values })),

  serverHistoryItemsReady: true,
  setServerHistoryItemsReady: (value) => set((state) => ({ serverHistoryItemsReady: value })),
  serverHistoryItems: [],
  setServerHistoryItems: (values) => set((state) => ({ serverHistoryItems: values })),

  // File System Items
  fileSystemItemsReady: true,
  setFileSystemItemsReady: (value) => set((state) => ({ fileSystemItemsReady: value })),
  fileSystemItems: [],
  setFileSystemItems: (values) => set((state) => ({ fileSystemItems: values })),

  fileSystemHistoryItemsReady: true,
  setFileSystemHistoryItemsReady: (value) =>
    set((state) => ({ fileSystemHistoryItemsReady: value })),
  fileSystemHistoryItems: [],
  setFileSystemHistoryItems: (values) => set((state) => ({ fileSystemHistoryItems: values })),
}));
