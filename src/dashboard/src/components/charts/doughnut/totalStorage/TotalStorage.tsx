'use client';

import { convertToStorageSize } from '@/utils';
import { ArcElement, Chart as ChartJS, Tooltip } from 'chart.js';
import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import styles from './TotalStorage.module.scss';
import { useTotalStorageDoughnutChart } from './hooks';

ChartJS.register(ArcElement, Tooltip);

export const TotalStorage: React.FC = () => {
  const data = useTotalStorageDoughnutChart();

  return (
    <div className={styles.panel}>
      <h1>Total Storage Allocation</h1>
      <div className={styles.chartContainer}>
        <div className={styles.chart}>
          <Doughnut
            data={data.chart}
            options={{
              rotation: 180,
              circumference: 360,
              cutout: '90%',
              plugins: {
                legend: {
                  display: false,
                },
                tooltip: {
                  callbacks: {
                    label: function (context: { dataIndex: any; parsed: any }) {
                      let labelIndex = context.dataIndex;
                      let label = data.chart.labels?.[labelIndex] || '';
                      let value = context.parsed;
                      return `${label}: ${convertToStorageSize(value, 'MB', 'TB')}`;
                    },
                    title: function () {
                      return ''; // Return an empty string to remove the title for tooltips
                    },
                  },
                },
              },
            }}
          />
        </div>
        <p className={styles.percentage}>
          <span>{data.usedPercent.toFixed(2)}%</span>Used
        </p>
        <p className={styles.total}>Total: {data.space}</p>
        <div className={styles.footer}>
          <p>Used: {data.used.replace(' ', '')}</p>
          <p>Unused: {data.available.replace(' ', '')}</p>
        </div>
      </div>
    </div>
  );
};
