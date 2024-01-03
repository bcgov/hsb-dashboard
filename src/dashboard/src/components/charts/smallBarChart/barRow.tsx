import Link from '@/../node_modules/next/link';
import React from 'react';
import styles from './smallBarChart.module.scss';

interface BarRowProps {
  operatingSystem: string;
  drive: string;
  allocated: string;
  used: string;
  unused: string;
  percentageUsed: number;
};

export const BarRow: React.FC<BarRowProps> = ({
  operatingSystem,
  drive,
  allocated,
  used,
  unused,
  percentageUsed,
}) => {
  return (
    <div className={styles.row}>
      <div className={styles.info}>
         {operatingSystem != "" ? <Link href="">{operatingSystem}</Link> : <p>{drive}</p>}
         <p>{allocated}</p>
         <p>{used}</p>
         <p>{unused}</p>
      </div>
      <div className={styles.barChart}>
        <div className={styles.bar}>
          {/* Applying width as an inline style */}
          <div className={styles.percentage} style={{ width: `${percentageUsed}%` }}></div>
        </div>
      </div>
      <p className={styles.used}>{percentageUsed}% of {allocated} Used</p>
    </div>
  );
};
