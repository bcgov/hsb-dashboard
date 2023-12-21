import {
  IConfigurationItemModel,
  IFileSystemItemModel,
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IFilteredState {
  // Date Range
  dateRange?: string[];
  setDateRange: (value?: string[]) => void;

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
  serverItem?: IServerItemModel;
  setServerItem: (value?: IServerItemModel) => void;
  serverItems: IServerItemModel[];
  setServerItems: (values: IServerItemModel[]) => void;

  // File System Items
  fileSystemItem?: IFileSystemItemModel;
  setFileSystemItem: (value?: IFileSystemItemModel) => void;
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;

  // Configuration Items
  configurationItem?: IConfigurationItemModel;
  setConfigurationItem: (value?: IConfigurationItemModel) => void;
  configurationItems: IConfigurationItemModel[];
  setConfigurationItems: (values: IConfigurationItemModel[]) => void;
}

export const useFiltered = create<IFilteredState>((set) => ({
  // Date Range
  setDateRange: (values) => set((state) => ({ dateRange: values })),

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
  setServerItem: (value) => set((state) => ({ serverItem: value })),
  serverItems: [],
  setServerItems: (values) => set((state) => ({ serverItems: values })),

  // File System Items
  setFileSystemItem: (value) => set((state) => ({ fileSystemItem: value })),
  fileSystemItems: [],
  setFileSystemItems: (values) => set((state) => ({ fileSystemItems: values })),

  // Configuration Items
  setConfigurationItem: (value) => set((state) => ({ configurationItem: value })),
  configurationItems: [],
  setConfigurationItems: (values) => set((state) => ({ configurationItems: values })),
}));
