'use client';

import { usePathname } from 'next/navigation';
import { AuthState } from '../auth';
import Image from 'next/image';
import './style.css';

export const Header: React.FC = () => {
  const path = usePathname();

  const isLogin = path.includes('/login');

  return (
    <header className="header test">
      <div className="flex-1 flex flex-row gap-4">
        <Image src="/images/BCLogo.png" alt="Logo" width={215} height={90} />
        <div className="border-l-2 p-2 text-2xl font-light">Storage Dashboard</div>
      </div>
      {!isLogin && <AuthState />}
    </header>
  );
};
