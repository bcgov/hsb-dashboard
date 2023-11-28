import React from 'react';

export interface IAppState {
  message?: string;
  setMessage?: (value?: string) => void;
}

const AppContext = React.createContext<IAppState>({});

interface IAppContextProps {
  initialState: IAppState;
  children: React.ReactNode;
}

export const AppContextProvider = ({ children }: IAppContextProps) => {
  const [message, setMessage] = React.useState<string>();

  return <AppContext.Provider value={{ message, setMessage }}>{children}</AppContext.Provider>;
};

export const useAppContext = () => {
  const context = React.useContext(AppContext);

  if (context === undefined) {
    throw new Error('useAppContext should be used within the AppContext provider!');
  }

  return context;
};
