'use client';

import React from 'react';
import styles from './OrganizationsChart.module.scss';
import { Button } from '@/components/buttons';
import { Select } from '@/components/forms/select';
import { Text } from '@/components/forms/text';
import { BarChart } from './BarChart';

const PercentageLines = () => {
  const percentages = Array.from({ length: 11 }, (_, index) => index * 10);

  return (
    <div className={styles.linesContainer}>
      {percentages.map((percentage) => (
        <div key={percentage} className={styles.percentageLine} style={{ left: `${percentage}%` }}>
          <div className={styles.line}></div>
          <div className={styles.label}>{`${percentage}%`}</div>
        </div>
      ))}
    </div>
  );
};

export const AllocationByStorageVolume: React.FC = () => {
  return (
    <div className={styles.panel}>
      <h1>Allocation by Storage Volume - All Organizations</h1>
      <div className={styles.sort}>
        <Select
          variant="primary"
          title="Sort options"
          options={[]}
          placeholder="Highest unused space"
        />
        <Text placeholder="Search" iconType={'search'}></Text>
        <Button variant="secondary">Search</Button>
      </div>
      <div className={styles.chartContainer}>
        <PercentageLines />
        <BarChart percentUsed={30} totalStorage={500}/>
        <BarChart percentUsed={40} totalStorage={400}/>
        <BarChart percentUsed={50} totalStorage={300}/>
        <BarChart percentUsed={60} totalStorage={200}/>
        <BarChart percentUsed={70} totalStorage={100}/>
        <BarChart percentUsed={80} totalStorage={50}/>
        <BarChart percentUsed={90} totalStorage={500}/>
        <BarChart percentUsed={95} totalStorage={200}/>
      </div>
      <div className={styles.footer}>
        <p>Used</p>
        <p>Unused</p>
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png">
        Export to Excel
      </Button>
    </div>
  );
};
