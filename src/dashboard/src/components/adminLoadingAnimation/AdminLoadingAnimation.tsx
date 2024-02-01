import styles from './AdminLoadingAnimation.module.scss';

export const AdminLoadingAnimation = () => {
  return (
      <div className={styles.panel}>
        <div className={styles.title}></div>
        <div className={styles.title}></div>
        <div className={styles.titleRow}>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
        </div>
        <div className={styles.rows}>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
        </div>
      </div>
  );
};
