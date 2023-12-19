import { create } from 'zustand';
import { createJSONStorage, persist } from 'zustand/middleware';

export interface IAppState {
  userinfo?: any;
  setUserinfo: (value: any) => void;
}

export const useApp = create<IAppState>((set) => ({
  userinfo: undefined,
  setUserinfo: (value: any) => set((state) => ({ userinfo: value })),
}));

export const useApp2 = create(
  persist(
    (set, get): IAppState => ({
      userinfo: undefined,
      setUserinfo: (value) => set((state: IAppState) => ({ userinfo: value })),
    }),
    {
      name: 'test',
      storage: createJSONStorage(() => sessionStorage),
    },
  ),
);
