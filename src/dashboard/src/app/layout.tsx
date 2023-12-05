import { Footer, Header, SessionProviderWrapper } from '@/components';
import type { Metadata } from 'next';
import './globals.css';
import "./styles/_fonts.scss";

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
          {children}
          <Footer />
        </body>
      </SessionProviderWrapper>
    </html>
  );
}
