'use client';

import style from './Header.module.scss';

import { useAuth } from '@/hooks';
import Image from 'next/image';
import Link from 'next/link';
import { redirect, usePathname } from 'next/navigation';
import React from 'react';
import { AuthState } from '../auth';
import { Filter } from '@/components/Filter';

export const Header: React.FC = () => {
  const path = usePathname();
  const {
    isLoading,
    isAuthenticated,
    isAuthorized,
    isSystemAdmin,
    isHSB,
    isClient,
    isOrganizationAdmin,
  } = useAuth();

  React.useEffect(() => {
    if (!isLoading) {
      if (!isAuthenticated && !path.includes('/login')) redirect('/login');
      else if (isAuthenticated && !isAuthorized && !path.includes('/welcome')) redirect('/welcome');
    }
  }, [isAuthenticated, isAuthorized, isLoading, path]);

  const isLogin = path.includes('/login');
  const rootPath = isHSB ? 'hsb' : 'client';

  const infoIcon = false;
  const isDashboardView = true;

  return (
    <>
      <header className={`${style.header} ${isDashboardView && style.filterPadding}`}>
        <div className={style.container}>
          <div className={style.headerTop}>
            <div>
              <Link href="/">
                <Image src="/images/BCLogo.png" alt="Logo" width={134} height={56} />
              </Link>
              <h1>Storage Dashboard</h1>
            </div>
            {!isLogin && <AuthState showName={true} />}
          </div>
          <div className={style.headerMiddle}>
            {infoIcon && <span className={style.infoIcon}></span>}
            <p>
              Welcome to the storage dashboard. This is an overview of the storage consumption for
              all organizations you belong to. <br />
              Use the filters to see further breakdowns of storage data.
            </p>
          </div>
          <div className={style.headerBottom}>
            {isAuthorized && (
              <>
                {(isClient || isHSB) && (
                  <>
                    <nav>
                      <Link
                        href={`/${rootPath}/dashboard`}
                        className={`${path.startsWith(`/${rootPath}/dashboard`) && style.active} ${
                          style.storage
                        }`}
                      >
                        Storage
                      </Link>
                      {(isOrganizationAdmin || isSystemAdmin) && (
                        <Link
                          href={`/${rootPath}/admin`}
                          className={`${path.startsWith(`/${rootPath}/admin`) && style.active} ${
                            style.admin
                          } `}
                        >
                          Administration
                        </Link>
                      )}
                    </nav>
                    {(isOrganizationAdmin || isSystemAdmin) && (
                      <>
                        {/* <Link href="/client/dashboard" className={style.allServers}>
                        See all servers
                      </Link> */}
                        <nav>
                          <div className={style.adminNav}>
                            <Link
                              href={`/${rootPath}/admin/organizations`}
                              title="Look up organizations and enable them on dashboard"
                              className={`${
                                path.startsWith(`/${rootPath}/admin/organizations`) && style.active
                              }`}
                            >
                              Organizations
                            </Link>
                            <Link
                              href={`/${rootPath}/admin/users`}
                              title="Manage user access to dashboard, assign roles"
                              className={`${
                                path.startsWith(`/${rootPath}/admin/users`) && style.active
                              }`}
                            >
                              Users
                            </Link>
                          </div>
                        </nav>
                        <span className={style.navLine}></span>
                      </>
                    )}
                  </>
                )}
                {isSystemAdmin && path.startsWith('/hsb/admin/organizations') && (
                  <nav className={style.adminSubNav}>
                    <Link
                      href="/hsb/admin/organizations"
                      className={`${style.subNavItem} ${
                        path === '/hsb/admin/organizations' && style.active
                      }`}
                    >
                      All Organizations
                    </Link>
                    <Link
                      href="/hsb/admin/organizations/0"
                      className={`${style.subNavItem} ${
                        path.startsWith('/hsb/admin/organizations/0') && style.active
                      }`}
                    >
                      Add New Organization
                    </Link>
                    <Link
                      href="/hsb/admin/organizations/1"
                      className={`${style.subNavItem} ${
                        /\/hsb\/admin\/organizations\/[1-9]+/.test(path) && style.active
                      }`}
                    >
                      Update Existing Organization
                    </Link>
                  </nav>
                )}
              </>
            )}
          </div>
        </div>
      </header>
      {isDashboardView && <Filter></Filter>}
    </>
  );
};
