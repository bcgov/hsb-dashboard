'use client';

import { LoadingAnimation } from '@/components';
import { useAuth } from '@/hooks';
import { useAppStore } from '@/store';
import { redirect } from 'next/navigation';
import styles from './Welcome.module.scss';

export default function Page() {
  const { isAuthorized } = useAuth();
  const userinfo = useAppStore((state) => state.userinfo);

  // Need this because after activating a pre-authorized user so that it will redirect to their home page.
  if (isAuthorized) redirect('/');

  return (
    <div className={`dashboardContainer ${styles.container}`}>
      <div className={styles.welcome}>
        <h2>Request Access</h2>
        <p>
          Please email <a href="mailto:placeholder@gov.bc.ca">placeholder@gov.bc.ca</a> to request
          access to your organization&apos;s dashboard
        </p>
        {!userinfo && (
          <div className={styles.activate}>
            <div>
              Activating account. If account has been pre-approved it will automatically apply roles
              and redirect to home page.
            </div>
            <div>
              <LoadingAnimation />
            </div>
          </div>
        )}
        {userinfo && !userinfo.roles.length && (
          <div className={styles.activate}>Your account has not be pre-approved.</div>
        )}
      </div>
      <div className={styles.login}>
        <h2>Welcome to the Storage Dashboard</h2>
        <p>Request access to get insights into your organization&apos;s data storage and usage.</p>
      </div>
    </div>
  );
}
