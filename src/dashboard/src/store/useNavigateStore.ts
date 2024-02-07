import { UrlObject } from 'url';
import { create } from 'zustand';

export interface INavigateStoreState {
  // Handle form navigation when form is dirty.
  showNavConfirmation: boolean;
  setShowNavConfirmation: (value: boolean) => void;
  enableNavigate: boolean;
  setEnableNavigate: (value: boolean) => void;
  navigateTo?: string | UrlObject;
  setNavigateTo: (value?: string | UrlObject) => void;
}

export const useNavigateStore = create<INavigateStoreState>((set) => ({
  // Handle form navigation when form is dirty.
  showNavConfirmation: false,
  setShowNavConfirmation: (value: boolean) => set((state) => ({ showNavConfirmation: value })),
  enableNavigate: true,
  setEnableNavigate: (value: boolean) => set((state) => ({ enableNavigate: value })),
  setNavigateTo: (value?: string | UrlObject) => set((state) => ({ navigateTo: value })),
}));
