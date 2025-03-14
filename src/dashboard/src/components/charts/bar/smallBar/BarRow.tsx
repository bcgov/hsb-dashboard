import React from 'react';
import { convertToStorageSize } from './../../../../utils/convertToStorageSize';
import { IBarChartRowData } from './IBarChartRowData';
import styles from './SmallBarChart.module.scss';
import { IFileSystemItemModel, IServerItemModel } from '@/hooks';

interface IBarRowProps extends Omit<IBarChartRowData<unknown>, 'label'> {
  label: React.ReactNode;
}

/**
 * Provides a single bar that displays percentage used.
 * @param param0 Component properties
 * @returns Component
 */
export const BarRow: React.FC<IBarRowProps> = (props) => {
  const { label, capacity, available } = props;

  const used = capacity - available;
  const percentageUsed = capacity ? Math.round((used / capacity) * 100) : 0;
  const capacityValue = convertToStorageSize(capacity, 'B', 'TB');
  const usedValue = convertToStorageSize(used, 'B', 'TB');
  const availableValue = convertToStorageSize(available, 'B', 'TB');

  const showDiskType = (props.data as IFileSystemItemModel)?.isSAN !== undefined;

  const diskType = showDiskType
    ? (props.data as IFileSystemItemModel)?.isSAN
      ? 'SAN'
      : 'Non-SAN'
    : '';

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        <p style={{ display: 'flex', alignItems: 'center' }}>
          {showDiskType && (
            <span className={diskType === 'SAN' ? styles.badgeYellow : styles.badge}>
              {diskType}
            </span>
          )}
          {label}{' '}
          <div
            style={{
              fontSize: '12px',
              marginLeft: '10px',
              border: 'solid 1px #999',
              borderRadius: '4px',
              padding: '1px 3px',
              color: '#999',
            }}
          >
            {props.data?.storageType}
          </div>
        </p>
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
