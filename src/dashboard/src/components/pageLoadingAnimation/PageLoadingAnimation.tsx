import styles from './PageLoadingAnimation.module.scss';

export const PageLoadingAnimation = () => {
  return (
    <>
      <div className={styles.panelFilter}>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
        <div></div>
      </div>
      <div className={styles.panel}>
        <div className={styles.title}></div>
        <div className={styles.line}></div>
        <div className={styles.line}></div>
        <div className={styles.line}></div>
        <div className={styles.line}></div>
        <div className={styles.circle}></div>
      </div>
      <div className={styles.panel}>
        <div className={styles.title}></div>
        <div className={styles.graph}>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
        </div>
      </div>
      <div className={styles.panel2}>
        <div className={styles.title}></div>
        <div className={styles.chart}>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
          <div></div>
        </div>
      </div>
    </>
  );
};
