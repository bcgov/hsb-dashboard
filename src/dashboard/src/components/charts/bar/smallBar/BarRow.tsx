import React from 'react';
import { convertToStorageSize } from './../../../../utils/convertToStorageSize';
import { IBarChartRowData } from './IBarChartRowData';
import styles from './SmallBarChart.module.scss';
import { IFileSystemItemModel, IServerItemModel } from '@/hooks';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

interface IBarRowProps extends Omit<IBarChartRowData<unknown>, 'label'> {
  label: React.ReactNode;
}

/**
 * Provides a single bar that displays percentage used.
 * @param param0 Component properties
 * @returns Component
 */
export const BarRow: React.FC<IBarRowProps> = (props) => {
  const { label, capacity, available, data } = props;

  const used = capacity - available;
  const percentageUsed = capacity ? Math.round((used / capacity) * 100) : 0;
  const capacityValue = convertToStorageSize(capacity, 'B', 'TB');
  const usedValue = convertToStorageSize(used, 'B', 'TB');
  const availableValue = convertToStorageSize(available, 'B', 'TB');

  const showDiskType = (data as IFileSystemItemModel)?.isSAN !== undefined;

  const diskType = showDiskType ? ((data as IFileSystemItemModel)?.isSAN ? 'SAN' : 'Non-SAN') : '';

  console.log('data', data);

  return (
    <div className={styles.row}>
      <div
        className={styles.info}
        style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          paddingRight: '20px',
          flexGrow: 1,
          width: '65%',
          minWidth: '65%',
        }}
      >
        <p
          style={{
            display: 'flex',
            alignItems: 'center',
            fontSize: '14px !important',
            flexGrow: 1,
            maxWidth: '300px',
            minWidth: '100px',
          }}
        >
          {showDiskType && (
            <span
              style={{ minWidth: '50px', textAlign: 'center' }}
              className={diskType === 'SAN' ? styles.badgeYellow : styles.badge}
            >
              {diskType}
            </span>
          )}
          {label}
        </p>
        <p
          style={{
            fontSize: '14px',
            whiteSpace: 'nowrap',
            minWidth: '80px',
            textAlign: 'right',
            flexShrink: 0,
          }}
        >
          {capacityValue}
        </p>
        <p
          style={{
            fontSize: '14px',
            whiteSpace: 'nowrap',
            minWidth: '80px',
            textAlign: 'right',
            flexShrink: 0,
          }}
        >
          {usedValue}
        </p>
        <p
          style={{
            fontSize: '14px',
            whiteSpace: 'nowrap',
            minWidth: '80px',
            textAlign: 'right',
            flexShrink: 0,
          }}
        >
          {availableValue}
        </p>
      </div>
      <div className={styles.barChart} style={{ width: '35%', minWidth: '35%', flexShrink: 0 }}>
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
