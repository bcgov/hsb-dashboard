import Link from 'next/link';
import React from 'react';
import styles from './AllocationTable.module.scss';

interface TableRowProps {
  server: string;
  tenant: string;
  os: string;
  allocated: number;
  unused: number;
}

export const TableRow: React.FC<TableRowProps> = ({ server, tenant, os, allocated, unused }) => {
  const percentageUsed = Math.round(((allocated - unused) / allocated) * 100);

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        <Link href={``}>{server}</Link>
        <p>{os}</p>
        {tenant ? <p>{tenant}</p> : ''}
        <p>{allocated.toFixed(1)} TB</p>
        <p>{unused.toFixed(1)} TB</p>
      </div>
      <div className={styles.barChart}>
        <div className={styles.bar}>
          {/* Applying width as an inline style */}
          <div className={styles.percentage} style={{ width: `${percentageUsed}%` }} />
        </div>
      </div>
      <p className={styles.used}>
        {percentageUsed}% of {allocated.toFixed(1)} TB Used
      </p>
    </div>
  );
};
