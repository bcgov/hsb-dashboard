import {
  IConfigurationItemModel,
  IFileSystemItemModel,
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IAppState {
  // User
  userinfo?: any; // TODO: Replace with interface.
  setUserinfo: (value: any) => void;

  // Tenants
  tenants: ITenantModel[];
  setTenants: (values: ITenantModel[]) => void;

  // Organizations
  organizations: IOrganizationModel[];
  setOrganizations: (values: IOrganizationModel[]) => void;

  // Operating System Items
  operatingSystemItems: IOperatingSystemItemModel[];
  setOperatingSystemItems: (values: IOperatingSystemItemModel[]) => void;

  // Server Items
  serverItems: IServerItemModel[];
  setServerItems: (values: IServerItemModel[]) => void;

  // File System Items
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;

  // Configuration Items
  configurationItems: IConfigurationItemModel[];
  setConfigurationItems: (values: IConfigurationItemModel[]) => void;
}

export const useApp = create<IAppState>((set) => ({
  // User
  userinfo: undefined,
  setUserinfo: (value) => set((state) => ({ userinfo: value })),

  // Tenants
  tenants: [],
  setTenants: (values) => set((state) => ({ tenants: values })),

  // Organizations
  organizations: [],
  setOrganizations: (values) => set((state) => ({ organizations: values })),

  // Operating System Items
  operatingSystemItems: [],
  setOperatingSystemItems: (values) => set((state) => ({ operatingSystemItems: values })),

  // Server Items
  serverItems: [],
  setServerItems: (values) => set((state) => ({ serverItems: values })),

  // File System Items
  fileSystemItems: [],
  setFileSystemItems: (values) => set((state) => ({ fileSystemItems: values })),

  // Configuration Items
  configurationItems: [],
  setConfigurationItems: (values) => set((state) => ({ configurationItems: values })),
}));

// export const useApp2 = create(
//   persist(
//     (set, get): IAppState => ({
//       userinfo: undefined,
//       setUserinfo: (value) => set((state: IAppState) => ({ userinfo: value })),
//       tenants: [],
//       setTenants: (tenants) => set((state: IAppState) => ({ tenants })),
//     }),
//     {
//       name: 'test',
//       storage: createJSONStorage(() => sessionStorage),
//     },
//   ),
// );
