import {
  IFileSystemItemModel,
  IOperatingSystemItemListModel,
  IOrganizationListModel,
  IServerItemListModel,
  ITenantListModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IDashboardStoreState {
  // Tenants
  tenant?: ITenantListModel;
  setTenant: (value?: ITenantListModel) => void;
  tenants: ITenantListModel[];
  setTenants: (values: ITenantListModel[]) => void;

  // Organizations
  organization?: IOrganizationListModel;
  setOrganization: (value?: IOrganizationListModel) => void;
  organizations: IOrganizationListModel[];
  setOrganizations: (values: IOrganizationListModel[]) => void;

  // Operating System Items
  operatingSystemItem?: IOperatingSystemItemListModel;
  setOperatingSystemItem: (value?: IOperatingSystemItemListModel) => void;
  operatingSystemItems: IOperatingSystemItemListModel[];
  setOperatingSystemItems: (values: IOperatingSystemItemListModel[]) => void;

  // Server Items
  serverItemsReady?: boolean;
  setServerItemsReady: (value?: boolean) => void;
  serverItem?: IServerItemListModel;
  setServerItem: (value?: IServerItemListModel) => void;
  serverItems: IServerItemListModel[];
  setServerItems: (values: IServerItemListModel[]) => void;

  // File System Items
  fileSystemItemsReady?: boolean;
  setFileSystemItemsReady: (value?: boolean) => void;
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;
}

export const useDashboardStore = create<IDashboardStoreState>((set) => ({
  // Tenants
  setTenant: (value) => set((state) => ({ tenant: value })),
  tenants: [],
  setTenants: (values) => set((state) => ({ tenants: values })),

  // Organizations
  setOrganization: (value) => set((state) => ({ organization: value })),
  organizations: [],
  setOrganizations: (values) => set((state) => ({ organizations: values })),

  // Operating System Items
  setOperatingSystemItem: (value) => set((state) => ({ operatingSystemItem: value })),
  operatingSystemItems: [],
  setOperatingSystemItems: (values) => set((state) => ({ operatingSystemItems: values })),

  // Server Items
  serverItemsReady: true,
  setServerItemsReady: (value) => set((state) => ({ serverItemsReady: value })),
  setServerItem: (value) => set((state) => ({ serverItem: value })),
  serverItems: [],
  setServerItems: (values) => set((state) => ({ serverItems: values })),

  // File System Items
  fileSystemItemsReady: true,
  setFileSystemItemsReady: (value) => set((state) => ({ fileSystemItemsReady: value })),
  fileSystemItems: [],
  setFileSystemItems: (values) => set((state) => ({ fileSystemItems: values })),
}));
