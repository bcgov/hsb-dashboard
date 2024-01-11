import Link from 'next/link';
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
}

export const TableRow: React.FC<TableRowProps> = ({
  server,
  tenant,
  os,
  capacity,
  available,
  showTenant,
}) => {
  const percentageUsed = capacity ? Math.round(((capacity - available) / capacity) * 100) : 0;
  const capacityValue = convertToStorageSize<string>(capacity, 'MB', 'TB');
  const availableValue = convertToStorageSize<string>(available, 'MB', 'TB');

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        <Link href={``} title={server}>
          {server}
        </Link>
        {showTenant ? <p title={tenant}>{tenant}</p> : ''}
        <p title={os}>{os}</p>
        <p title={capacityValue}>{capacityValue}</p>
        <p title={availableValue}>{availableValue}</p>
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
