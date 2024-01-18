import { convertToStorageSize } from '@/utils';
import Link from 'next/link';
import styles from './AllocationByStorageVolume.module.scss';

export interface IBarChartProps {
  label: string;
  to?: string;
  availableSpace: number;
  totalStorage: number;
  onClick?: (e: React.MouseEvent<HTMLLabelElement, MouseEvent>) => void;
}

export const BarChart: React.FC<IBarChartProps> = ({
  label,
  to = '',
  availableSpace,
  totalStorage,
  onClick,
}) => {
  var validPercentage = totalStorage
    ? Math.min(100, Math.max(0, Math.round(((totalStorage - availableSpace) / totalStorage) * 100)))
    : 0;
  const usedStorage = totalStorage - availableSpace;
  const usedStorageLabel = convertToStorageSize(usedStorage, 'MB', 'TB', {
    formula: (value) => Number(value.toFixed(2)),
  });
  const totalStorageLabel = convertToStorageSize(totalStorage, 'MB', 'TB', {
    formula: (value) => Number(value.toFixed(2)),
  });

  return (
    <div className={styles.barChart}>
      {to ? (
        <Link href={to}>{label}</Link>
      ) : onClick ? (
        <label className={styles.link} onClick={(e) => onClick?.(e)}>
          {label}
        </label>
      ) : (
        <label>{label}</label>
      )}
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
      <p>
        {usedStorageLabel} of {totalStorageLabel} Used
      </p>
    </div>
  );
};
