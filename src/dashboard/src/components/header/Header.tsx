'use client';

import Image from 'next/image';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { AuthState } from '../auth';
import './style.css';

export const Header: React.FC = () => {
  const path = usePathname();

  const isLogin = path.includes('/login');

  return (
    <header className="header">
      <div className="flex-1 flex flex-row gap-4">
        <Link href="/">
          <Image src="/images/BCLogo.png" alt="Logo" width={215} height={90} />
        </Link>
        <div className="border-l-2 p-2 text-2xl font-light">Storage Dashboard</div>
      </div>
      {!isLogin && <AuthState />}
    </header>
  );
};
