import styles from './AllocationByStorageVolume.module.scss';

export const PercentageLines = () => {
  const percentages = Array.from({ length: 11 }, (_, index) => index * 10);

  return (
    <div className={styles.linesContainer}>
      {percentages.map((percentage) => (
        <div key={percentage} className={styles.percentageLine} style={{ left: `${percentage}%` }}>
          <div className={styles.line}></div>
          <div className={styles.label}>{`${percentage}%`}</div>
        </div>
      ))}
    </div>
  );
};
