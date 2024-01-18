import React from 'react';
import { convertToStorageSize } from './../../../../utils/convertToStorageSize';
import { IBarChartRowData } from './IBarChartRowData';
import styles from './SmallBarChart.module.scss';

interface IBarRowProps extends Omit<IBarChartRowData<unknown>, 'label'> {
  label: React.ReactNode;
}

export const BarRow: React.FC<IBarRowProps> = ({ label, capacity, available }) => {
  const used = capacity - available;
  const percentageUsed = capacity ? Math.round((used / capacity) * 100) : 0;
  const capacityValue = convertToStorageSize(capacity, 'MB', 'TB');
  const usedValue = convertToStorageSize(used, 'MB', 'TB');
  const availableValue = convertToStorageSize(available, 'MB', 'TB');

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        {label}
        <p>{capacityValue}</p>
        <p>{usedValue}</p>
        <p>{availableValue}</p>
      </div>
      <div className={styles.barChart}>
        <div className={styles.bar}>
          {/* Applying width as an inline style */}
          <div className={styles.percentage} style={{ width: `${percentageUsed}%` }} />
        </div>
      </div>
      <p className={styles.used}>
        {percentageUsed}% of {capacityValue} Used
      </p>
    </div>
  );
};
