import React from 'react';
import { convertToStorageSize } from './../../../utils/convertToStorageSize';
import styles from './AllocationTable.module.scss';
import { calcPercentageUsed, calcPercentageUnused } from '@/utils/calcPercentageUsed';

interface TableRowProps {
  server: string;
  tenant: string;
  os: string;
  capacity: number;
  available: number;
  showTenant?: boolean;
  showAbsolute?: boolean;
  onClick?: (e: React.MouseEvent<HTMLLabelElement, MouseEvent>) => void;
}

export const TableRow: React.FC<TableRowProps> = ({
  server,
  tenant,
  os,
  capacity,
  available,
  showTenant,
  showAbsolute,
  onClick,
}) => {
  const percentageUsed = calcPercentageUsed(available, capacity);
  const percentageUnused = calcPercentageUnused(available, capacity);
  const capacityValue = convertToStorageSize<string>(capacity, 'B', 'TB');
  const availableValue = convertToStorageSize<string>(available, 'B', 'TB');

  const handleClick = (e: React.MouseEvent<HTMLLabelElement, MouseEvent>) => {
    if (onClick) {
      onClick(e);
    }

    window.scrollTo({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  };

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        <p>
          {onClick ? (
            <label className={styles.link} onClick={handleClick}>
              {server}
            </label>
          ) : (
            <label>{server}</label>
          )}
        </p>
        {showTenant ? <p title={tenant}>{tenant}</p> : ''}
        <p title={os}>{os}</p>
        <p className={styles.centered} title={capacityValue}>
          {capacityValue}
        </p>
        <p className={styles.centered} title={availableValue}>
          {showAbsolute ? availableValue : percentageUnused + '%'}
        </p>
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
