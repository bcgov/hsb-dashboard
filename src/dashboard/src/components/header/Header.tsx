'use client';

import Image from 'next/image';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { AuthState } from '../auth';
import Image from 'next/image';
import Link from 'next/link';
import style from './Header.module.scss';

export const Header: React.FC = () => {
  const path = usePathname();

  const isLogin = path.includes('/login');

  const isAdmin = true;
  const isHSBAdmin = true;
  const isHSBAdminOrganizations = true;
  const infoIcon = false;

  return (
    <header className={style.header}>
      <div className={style.container}>
        <div className={style.headerTop}>
          <div>
            <Link href="">
              <Image src="/images/BCLogo.png" alt="Logo" width={134} height={56} />
            </Link>
            <h1>Storage Dashboard</h1>
          </div>
          {!isLogin && <AuthState />}
        </div>
        <div className={style.headerMiddle}>
          {infoIcon && <span className={style.infoIcon}></span>}
          <p>Welcome to the storage dashboard.  This is an overview of the storage consumption for all organizations you belong to.  <br/>Use the filters to see further breakdowns of storage data.</p>
        </div>
        <div className={style.headerBottom}>
          <nav>
            <a href="" className={`${!isAdmin && style.active} ${style.storage}`}>Storage</a>
            <a href="" className={`${isAdmin && style.active} ${style.admin} `}>Administration</a>
          </nav>
          {!isAdmin && <a href="" className={style.allServers}>See all servers</a>}
          {isAdmin && isHSBAdmin && ( 
            <>
              <nav>
                <div className={style.adminNav}>
                  <a href="" title="Look up organizations and enable them on dashboard" className={style.active}>Organizations</a>
                  <a href="" title="Manage user access to dashboard, assign roles">Users</a>
                </div>
              </nav>
              <span className={style.navLine}></span>
            </>
          )}
          {isAdmin && isHSBAdmin && isHSBAdminOrganizations && (
            <nav className={style.adminSubNav}>
                <a href="" className={`${style.subNavItem} ${style.active}`}>All Organizations</a>
                <a href="" className={`${style.subNavItem}`}>Add New Organization</a>
                <a href="" className={`${style.subNavItem}`}>Update Existing Organization</a>
            </nav>
          )}
        </div>
      </div>
    </header>
  );
};
