import styles from './LoadingAnimation.module.scss';

export const LoadingAnimation = ({
  className,
  ...rest
}: React.AllHTMLAttributes<HTMLDivElement>) => {
  return (
    <div className={`${styles.container} ${className} loadingAnimation`} {...rest}>
      <div className={styles.animationContainer}>
        <div className={styles.bar1}></div>
        <div className={styles.bar2}></div>
        <div className={styles.bar3}></div>
        <div className={styles.bar4}></div>
        <div className={styles.bar5}></div>
        <div className={styles.bar6}></div>
      </div>
    </div>
  );
};
