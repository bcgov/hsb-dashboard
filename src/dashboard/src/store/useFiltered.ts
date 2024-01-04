import {
  IFileSystemItemModel,
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IFilteredState {
  // Date Range
  dateRange: string[];
  setDateRange: (value?: string[]) => void;

  // Tenants
  tenantsReady?: boolean;
  setTenantsReady: (value?: boolean) => void;
  tenant?: ITenantModel;
  setTenant: (value?: ITenantModel) => void;
  tenants: ITenantModel[];
  setTenants: (values: ITenantModel[]) => void;

  // Organizations
  organizationsReady?: boolean;
  setOrganizationsReady: (value?: boolean) => void;
  organization?: IOrganizationModel;
  setOrganization: (value?: IOrganizationModel) => void;
  organizations: IOrganizationModel[];
  setOrganizations: (values: IOrganizationModel[]) => void;

  // Operating System Items
  operatingSystemItemsReady?: boolean;
  setOperatingSystemItemsReady: (value?: boolean) => void;
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
  fileSystemItem?: IFileSystemItemModel;
  setFileSystemItem: (value?: IFileSystemItemModel) => void;
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;
}

export const useFiltered = create<IFilteredState>((set) => ({
  // Date Range
  dateRange: [],
  setDateRange: (values) => set((state) => ({ dateRange: values })),

  // Tenants
  setTenantsReady: (value) => set((state) => ({ tenantsReady: value })),
  setTenant: (value) => set((state) => ({ tenant: value })),
  tenants: [],
  setTenants: (values) => set((state) => ({ tenants: values })),

  // Organizations
  setOrganizationsReady: (value) => set((state) => ({ organizationsReady: value })),
  setOrganization: (value) => set((state) => ({ organization: value })),
  organizations: [],
  setOrganizations: (values) => set((state) => ({ organizations: values })),

  // Operating System Items
  setOperatingSystemItemsReady: (value) => set((state) => ({ operatingSystemItemsReady: value })),
  setOperatingSystemItem: (value) => set((state) => ({ operatingSystemItem: value })),
  operatingSystemItems: [],
  setOperatingSystemItems: (values) => set((state) => ({ operatingSystemItems: values })),

  // Server Items
  setServerItemsReady: (value) => set((state) => ({ serverItemsReady: value })),
  setServerItem: (value) => set((state) => ({ serverItem: value })),
  serverItems: [],
  setServerItems: (values) => set((state) => ({ serverItems: values })),

  // File System Items
  setFileSystemItemsReady: (value) => set((state) => ({ fileSystemItemsReady: value })),
  setFileSystemItem: (value) => set((state) => ({ fileSystemItem: value })),
  fileSystemItems: [],
  setFileSystemItems: (values) => set((state) => ({ fileSystemItems: values })),
}));
