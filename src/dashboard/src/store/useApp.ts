import {
  IFileSystemItemModel,
  IGroupModel,
  IOperatingSystemItemModel,
  IOrganizationModel,
  IRoleModel,
  IServerItemModel,
  ITenantModel,
  IUserInfoModel,
  IUserModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IAppState {
  // User
  userinfo?: IUserInfoModel; // TODO: Replace with interface.
  setUserinfo: (value: IUserInfoModel) => void;

  // Roles
  roles: IRoleModel[];
  setRoles: (values: IRoleModel[]) => void;

  // Groups
  groups: IGroupModel[];
  setGroups: (values: IGroupModel[]) => void;

  // Users
  users: IUserModel[];
  setUsers: (values: IUserModel[]) => void;

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
}

export const useApp = create<IAppState>((set) => ({
  // User
  userinfo: undefined,
  setUserinfo: (value) => set((state) => ({ userinfo: value })),

  // Roles
  roles: [],
  setRoles: (values) => set((state) => ({ roles: values })),

  // Groups
  groups: [],
  setGroups: (values) => set((state) => ({ groups: values })),

  // Users
  users: [],
  setUsers: (values) => set((state) => ({ users: values })),

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
}));
