import styles from './Welcome.module.scss';

export default function Page() {
  return (
    <div className={`dashboardContainer ${styles.container}`}>
      <div className={styles.welcome}>
        <h2>Request Access</h2>
        <p>Please email <a href="mailto:placeholder@gov.bc.ca">placeholder@gov.bc.ca</a> to request access to your organization's dashboard</p>
      </div>
      <div className={styles.login}>
        <h2>Welcome to the Storage Dashboard</h2>
        <p>Sign in to get insights into your organization's data storage and usage.</p>
      </div>
    </div>
  );
}
