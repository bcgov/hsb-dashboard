import { Footer, Header, SessionProviderWrapper } from '@/components';
import type { Metadata } from 'next';
import { Source_Sans_3 } from 'next/font/google';
import './globals.css';

const inter = Source_Sans_3({ subsets: ['latin'] });

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
        <body className={`${inter.className} bg-gray`}>
          <Header />
          {children}
          <Footer />
        </body>
      </SessionProviderWrapper>
    </html>
  );
}
