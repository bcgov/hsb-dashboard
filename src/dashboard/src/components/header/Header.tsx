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

  const isHSBAdmin = true;
  const filtersApplied = false;

  return (
    <header className={style.header}>
      <div className={style.container}>
        <div className={style.headerTop}>
          <div>
          <Image src="/images/BCLogo.png" alt="Logo" width={134} height={56} />
          <h1>Storage Dashboard</h1>
          </div>
          {!isLogin && <AuthState />}
        </div>
        <div className={style.headerMiddle}>
          { filtersApplied && <span className={style.infoIcon}></span>}
          <p>Welcome to the storage dashboard.  This is an overview of the storage consumption for all organizations you belong to.  <br/>Use the filters to see further breakdowns of storage data.</p>
        </div>
        <div className={style.headerBottom}>
          <nav>
            <a href="" className={`${!isHSBAdmin && style.active} ${style.storage}`}>Storage</a>
            <a href="" className={`${isHSBAdmin && style.active} ${style.admin} `}>Administration</a>
          </nav>
          {isHSBAdmin && ( 
            <>
              <nav>
                <div className={style.adminNav}>
                  <a href="" className={style.active}>Organizations</a>
                  <a href="">Users</a>
                </div>
              </nav>
              <span className={style.navLine}></span>
            </>
          )}
        </div>
      </div>
    </header>
  );
};
