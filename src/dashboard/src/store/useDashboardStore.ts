import {
  IFileSystemItemModel,
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IDashboardStoreState {
  // Tenants
  tenant?: ITenantModel;
  setTenant: (value?: ITenantModel) => void;
  tenants: ITenantModel[];
  setTenants: (values: ITenantModel[]) => void;

  // Organizations
  organization?: IOrganizationModel;
  setOrganization: (value?: IOrganizationModel) => void;
  organizations: IOrganizationModel[];
  setOrganizations: (values: IOrganizationModel[]) => void;

  // Operating System Items
  operatingSystemItem?: IOperatingSystemItemModel;
  setOperatingSystemItem: (value?: IOperatingSystemItemModel) => void;
  operatingSystemItems: IOperatingSystemItemModel[];
  setOperatingSystemItems: (values: IOperatingSystemItemModel[]) => void;

  // Server Items
  serverItemsReady?: boolean;
  setServerItemsReady: (value?: boolean) => void;
  serverItem?: IServerItemModel;
  setServerItem: (value?: IServerItemModel) => void;
  serverItems: IServerItemModel[];
  setServerItems: (values: IServerItemModel[]) => void;

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
