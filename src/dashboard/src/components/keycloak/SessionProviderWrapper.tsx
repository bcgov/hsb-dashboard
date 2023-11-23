'use client';

import { SessionProvider } from 'next-auth/react';
import React from 'react';

export interface ISessionProviderWrapperProps {
  children: React.ReactNode;
}

export const SessionProviderWrapper: React.FC<ISessionProviderWrapperProps> = ({ children }) => {
  return <SessionProvider>{children}</SessionProvider>;
};
