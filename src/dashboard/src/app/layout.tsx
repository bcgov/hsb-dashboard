import { Footer, Header, SessionProviderWrapper } from '@/components';
import '@/styles/_fonts.scss';
import '@/styles/globals.scss';
import type { Metadata } from 'next';
import React from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const metadata: Metadata = {
  title: 'Hosting Storage Dashboard',
  description: 'Visualize their storage allocation and consumption',
};

export default async function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <SessionProviderWrapper>
        <head>
          <link rel="icon" href="/favicon.ico" sizes="any" />
        </head>
        <body className="bg-gray">
          <Header />
          <div className="dashboardContainer">{children}</div>
          <ToastContainer />
          <Footer />
        </body>
      </SessionProviderWrapper>
    </html>
  );
}
