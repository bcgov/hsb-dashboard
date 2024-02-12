import React from 'react';
import { convertToStorageSize } from './../../../utils/convertToStorageSize';
import styles from './AllocationTable.module.scss';

interface TableRowProps {
  server: string;
  tenant: string;
  os: string;
  capacity: number;
  available: number;
  showTenant?: boolean;
  onClick?: (e: React.MouseEvent<HTMLLabelElement, MouseEvent>) => void;
}

export const TableRow: React.FC<TableRowProps> = ({
  server,
  tenant,
  os,
  capacity,
  available,
  showTenant,
  onClick,
}) => {
  const percentageUsed = capacity ? Math.round(((capacity - available) / capacity) * 100) : 0;
  const capacityValue = convertToStorageSize<string>(capacity, 'B', 'TB');
  const availableValue = convertToStorageSize<string>(available, 'B', 'TB');

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        <p>
          {onClick ? (
            <label className={styles.link} onClick={(e) => onClick?.(e)}>
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
          {availableValue}
        </p>
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
