import {
  IFileSystemItemModel,
  IOperatingSystemItemListModel,
  IOrganizationListModel,
  IServerItemListModel,
  ITenantListModel,
} from '@/hooks/api';
import { create } from 'zustand';

export interface IFilterValues {
  tenant?: ITenantListModel;
  organization?: IOrganizationListModel;
  operatingSystemItem?: IOperatingSystemItemListModel;
  serverItem?: IServerItemListModel;
}

export interface IFilteredStoreState {
  // Filter
  loading: boolean;
  setLoading: (value: boolean) => void;
  values: IFilterValues;
  setValues: (dispatch: (values: IFilterValues) => IFilterValues) => void;

  // Tenants
  loadingTenants: boolean;
  setLoadingTenants: (value: boolean) => void;
  tenantsReady?: boolean;
  setTenantsReady: (value?: boolean) => void;
  tenants: ITenantListModel[];
  setTenants: (values: ITenantListModel[]) => void;

  // Organizations
  loadingOrganizations: boolean;
  setLoadingOrganizations: (value: boolean) => void;
  organizationsReady?: boolean;
  setOrganizationsReady: (value?: boolean) => void;
  organizations: IOrganizationListModel[];
  setOrganizations: (values: IOrganizationListModel[]) => void;

  // Operating System Items
  loadingOperatingSystemItems: boolean;
  setLoadingOperatingSystemItems: (value: boolean) => void;
  operatingSystemItemsReady?: boolean;
  setOperatingSystemItemsReady: (value?: boolean) => void;
  operatingSystemItems: IOperatingSystemItemListModel[];
  setOperatingSystemItems: (values: IOperatingSystemItemListModel[]) => void;

  // Server Items
  loadingServerItems: boolean;
  setLoadingServerItems: (value: boolean) => void;
  serverItemsReady?: boolean;
  setServerItemsReady: (value?: boolean) => void;
  serverItems: IServerItemListModel[];
  setServerItems: (values: IServerItemListModel[]) => void;

  // File System Items
  loadingFileSystemItems: boolean;
  setLoadingFileSystemItems: (value: boolean) => void;
  fileSystemItemsReady?: boolean;
  setFileSystemItemsReady: (value?: boolean) => void;
  fileSystemItems: IFileSystemItemModel[];
  setFileSystemItems: (values: IFileSystemItemModel[]) => void;
}

export const useFilteredStore = create<IFilteredStoreState>((set, get) => ({
  // Filter
  loading: false,
  setLoading: (value) => set((state) => ({ ...state, loading: value })),
  values: {},
  setValues: (dispatch: (values: IFilterValues) => IFilterValues) => {
    const values = dispatch(get().values);
    set((state) => ({ ...state, values: values ?? {} }));
  },

  // Tenants
  loadingTenants: false,
  setLoadingTenants: (value) => set((state) => ({ ...state, loadingTenants: value })),
  setTenantsReady: (value) => set((state) => ({ ...state, tenantsReady: value })),
  tenants: [],
  setTenants: (values) => set((state) => ({ ...state, tenants: values })),

  // Organizations
  loadingOrganizations: false,
  setLoadingOrganizations: (value) => set((state) => ({ ...state, loadingOrganizations: value })),
  setOrganizationsReady: (value) => set((state) => ({ ...state, organizationsReady: value })),
  organizations: [],
  setOrganizations: (values) => set((state) => ({ ...state, organizations: values })),

  // Operating System Items
  loadingOperatingSystemItems: false,
  setLoadingOperatingSystemItems: (value) =>
    set((state) => ({ ...state, loadingOperatingSystemItems: value })),
  setOperatingSystemItemsReady: (value) =>
    set((state) => ({ ...state, operatingSystemItemsReady: value })),
  operatingSystemItems: [],
  setOperatingSystemItems: (values) => set((state) => ({ ...state, operatingSystemItems: values })),

  // Server Items
  loadingServerItems: false,
  setLoadingServerItems: (value) => set((state) => ({ ...state, loadingServerItems: value })),
  setServerItemsReady: (value) => set((state) => ({ ...state, serverItemsReady: value })),
  serverItems: [],
  setServerItems: (values) => set((state) => ({ ...state, serverItems: values })),

  // File System Items
  loadingFileSystemItems: false,
  setLoadingFileSystemItems: (value) =>
    set((state) => ({ ...state, loadingFileSystemItems: value })),
  setFileSystemItemsReady: (value) => set((state) => ({ ...state, fileSystemItemsReady: value })),
  fileSystemItems: [],
  setFileSystemItems: (values) => set((state) => ({ ...state, fileSystemItems: values })),
}));
