import {
  IFileSystemItemModel,
  IGroupModel,
  IOperatingSystemItemListModel,
  IOrganizationListModel,
  IRoleModel,
  IServerItemListModel,
  ITenantListModel,
  IUserModel,
} from '@/hooks/api';
import { IUserInfoModel } from '@/hooks/api/interfaces/auth';
import { create } from 'zustand';

export interface IAppStoreState {
  // User
  userinfo?: IUserInfoModel;
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
  tenants: ITenantListModel[];
  setTenants: (values: ITenantListModel[]) => void;

  // Organizations
  organizations: IOrganizationListModel[];
  setOrganizations: (values: IOrganizationListModel[]) => void;

  // Operating System Items
  operatingSystemItems: IOperatingSystemItemListModel[];
  setOperatingSystemItems: (values: IOperatingSystemItemListModel[]) => void;

  // Server Items
  serverItems: IServerItemListModel[];
  setServerItems: (values: IServerItemListModel[]) => void;

  // File System Items
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;
}

export const useAppStore = create<IAppStoreState>((set) => ({
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
