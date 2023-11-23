import { AuthState } from '@/components/auth';
import { SessionProviderWrapper } from '@/components/keycloak';
import type { Metadata } from 'next';
import { Inter } from 'next/font/google';
import './globals.css';

const inter = Inter({ subsets: ['latin'] });

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
        <body className={inter.className}>
          <header>
            Hosting Storage Dashboard
            <AuthState />
          </header>
          {children}
          <footer>footer</footer>
        </body>
      </SessionProviderWrapper>
    </html>
  );
}
