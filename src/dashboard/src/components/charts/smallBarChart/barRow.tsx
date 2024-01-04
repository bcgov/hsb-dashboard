import Link from 'next/link';
import React from 'react';
import styles from './smallBarChart.module.scss';

interface BarRowProps {
  operatingSystem: string;
  drive: string;
  used: number;
  unused: number;
}

export const BarRow: React.FC<BarRowProps> = ({ operatingSystem, drive, used, unused }) => {
  const allocated = used + unused;
  const percentageUsed = Math.round((used / allocated) * 100);

  return (
    <div className={styles.row}>
      <div className={styles.info}>
        {operatingSystem ? <Link href={``}>{operatingSystem}</Link> : <p>{drive}</p>}
        <p>{allocated.toFixed(1)} TB</p>
        <p>{used.toFixed(1)} TB</p>
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
