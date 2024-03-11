'use client';

import styles from './NotAuthorized.module.scss';

export default function Page() {
  return (
    <div className={`dashboardContainer ${styles.container}`}>
      <div className={styles.welcome}>
        <h1>Not Authorized</h1>
        <p>Your account is not authorized to access the Storage Dashboard.</p>
      </div>
      <div className={styles.login}>
        <div>
          <h3>Need access to the Storage Dashboard?</h3>
          <p>
            Please email <a href="michael.tessier@gov.bc.ca">michael.tessier@gov.bc.ca</a> to
            request access to your organization&apos;s dashboard.
          </p>
        </div>
      </div>
    </div>
  );
}
