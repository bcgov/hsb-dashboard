import styles from './OrganizationsChart.module.scss';
import Link from 'next/link';

type BarChartProps = {
  percentUsed: number;
  totalStorage: number;
};

export const BarChart: React.FC<BarChartProps> = ({ percentUsed, totalStorage }) => {

  const validPercentage = Math.min(100, Math.max(0, percentUsed));

  const usedStorage = (validPercentage / 100) * totalStorage;
  
  return (
    <div className={styles.barChart}>
      <Link href="/">Organization Name</Link>
      <div className={styles.barLine}>
        <div
          className={styles.percentage}
          style={{ width: `${validPercentage}%` }} // Use inline style to set width
        >
          <div className={styles.tooltip}>
            <p>{validPercentage}% used</p>
          </div>
        </div>
      </div>
      <p>{usedStorage.toFixed(2)} TB of {totalStorage} TB Used</p>
    </div>
  );
};
