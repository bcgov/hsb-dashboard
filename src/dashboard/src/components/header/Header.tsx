'use client';

import Image from 'next/image';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { AuthState } from '../auth';
import Image from 'next/image';
import style from './Header.module.scss';

export const Header: React.FC = () => {
  const path = usePathname();

  const isLogin = path.includes('/login');

  return (
    <header className={style.header}>
      <div className={style.container}>
        <div className={style.headerTop}>
          <div>
          <Image src="/images/BCLogo.png" alt="Logo" width={215} height={90} />
          <h1>Storage Dashboard</h1>
          </div>
          {!isLogin && <AuthState />}
        </div>
        <div className={style.headerMiddle}>
          <p>Welcome to the storage dashboard.  This is an overview of the storage consumption for all organizations you belong to.  Use the filters to see further breakdowns of storage data.</p>
        </div>
        <div className={style.headerBottom}>
          <nav>
            <a href="" className={style.active}>Storage</a>
            <a href="">Administration</a>
          </nav>
        </div>
      </div>
    </header>
  );
};
