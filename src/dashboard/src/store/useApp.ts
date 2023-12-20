import { IOrganizationModel, ITenantModel } from '@/hooks/api';
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
