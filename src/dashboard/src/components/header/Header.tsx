'use client';

import style from './Header.module.scss';

import { Filter } from '@/components/filter';
import { useAuth } from '@/hooks';
import Image from 'next/image';
import { redirect, usePathname } from 'next/navigation';
import React from 'react';
import { ConfirmLink, ConfirmPopup } from '..';
import { AuthState } from '../auth';
import { Message } from './Message';

/**
 * Provides the header for the application with the navigation.
 * @returns Component
 */
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
  const isDashboardView = path.includes('/dashboard');
  const isServerView = path.includes('/servers');
  const isAdminView = path.includes('/admin');
  const showAdminNav = isDashboardView || isAdminView || isServerView;

  return (
    <>
      <ConfirmPopup />
      <header
        className={`${style.header} ${isDashboardView ? style.filterPadding : ''} ${
          isAdminView || isServerView ? style.adminPadding : ''
        }`}
      >
        <div className={style.container}>
          <div
            className={`${
              (path.startsWith(`/login`) || path.startsWith(`/welcome`)) && style.login
            } ${style.headerTop}`}
          >
            <div>
              <ConfirmLink href="/">
                <Image src="/images/BCLogo.png" alt="Logo" width={134} height={56} />
              </ConfirmLink>
              <h1>Storage Dashboard</h1>
            </div>
            {!isLogin && <AuthState showName={true} />}
          </div>
          {!(path.includes(`/login`) || path.includes(`/welcome`)) && (
            <>
              <div className={style.headerMiddle}>
                {infoIcon && <span className={style.infoIcon}></span>}
                <Message />
              </div>
              <div className={style.headerBottom}>
                {isAuthorized && (
                  <>
                    {(isClient || isHSB) && (
                      <>
                        <nav>
                          <ConfirmLink
                            href={`/${rootPath}/dashboard`}
                            className={`${
                              path.startsWith(`/${rootPath}/dashboard`) && style.active
                            } ${style.storage}`}
                          >
                            Storage
                          </ConfirmLink>
                          {(isOrganizationAdmin || isSystemAdmin) && showAdminNav && (
                            <ConfirmLink
                              href={`/${rootPath}/admin/users`}
                              className={`${
                                path.startsWith(`/${rootPath}/admin`) && style.active
                              } ${style.admin} `}
                            >
                              Administration
                            </ConfirmLink>
                          )}
                        </nav>
                        {(isDashboardView || isServerView) && (
                          <ConfirmLink href={`/${rootPath}/servers`} className={style.allServers}>
                            See all servers
                          </ConfirmLink>
                        )}
                        {isSystemAdmin && !isDashboardView && !isServerView && isAdminView && (
                          <>
                            <nav>
                              <div className={style.adminNav}>
                                <ConfirmLink
                                  href={`/${rootPath}/admin/organizations`}
                                  title="Look up organizations and enable them on dashboard"
                                  className={`${
                                    path.startsWith(`/${rootPath}/admin/organizations`) &&
                                    style.active
                                  }`}
                                >
                                  Organizations
                                </ConfirmLink>
                                <ConfirmLink
                                  href={`/${rootPath}/admin/users`}
                                  title="Manage user access to dashboard, assign roles"
                                  className={`${
                                    path.startsWith(`/${rootPath}/admin/users`) && style.active
                                  }`}
                                >
                                  Users
                                </ConfirmLink>
                              </div>
                            </nav>
                            <span className={style.navLine}></span>
                          </>
                        )}
                      </>
                    )}
                    {/* No need for subnav for MVP */}
                    {/* {isSystemAdmin && path.startsWith('/hsb/admin/organizations') && (
                      <nav className={style.adminSubNav}>
                        <ConfirmLink
                          href="/hsb/admin/organizations"
                          className={`${style.subNavItem} ${
                            path === '/hsb/admin/organizations' && style.active
                          }`}
                        >
                          All Organizations
                        </ConfirmLink>
                        <ConfirmLink
                          href="/hsb/admin/organizations/0"
                          className={`${style.subNavItem} ${
                            path.startsWith('/hsb/admin/organizations/0') && style.active
                          }`}
                        >
                          Add New Organization
                        </ConfirmLink>
                        <ConfirmLink
                          href="/hsb/admin/organizations/1"
                          className={`${style.subNavItem} ${
                            /\/hsb\/admin\/organizations\/[1-9]+/.test(path) && style.active
                          }`}
                        >
                          Update Existing Organization
                        </ConfirmLink>
                      </nav>
                    )} */}
                  </>
                )}
              </div>
            </>
          )}
        </div>
      </header>
      {(isClient || isHSB) && isDashboardView && <Filter />}
    </>
  );
};
