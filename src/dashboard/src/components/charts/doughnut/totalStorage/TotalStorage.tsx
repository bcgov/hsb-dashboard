'use client';

import { ArcElement, Chart as ChartJS, Tooltip } from 'chart.js';
import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import styles from './TotalStorage.module.scss';
import { defaultData } from './defaultData';

ChartJS.register(ArcElement, Tooltip);

export const TotalStorage: React.FC = () => {
  const data = defaultData;

  return (
    <div className={styles.panel}>
      <h1>Total Storage Allocation</h1>
      <div className={styles.chartContainer}>
        <div className={styles.chart}>
          <Doughnut
            data={data}
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
                      let label = data.labels[labelIndex] || '';
                      let value = context.parsed;
                      return `${label}: ${value}GB`;
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
          <span>{data.percentage.toFixed(0)}%</span>Used
        </p>
        <p className={styles.total}>Total: {data.space}GB</p>
        <div className={styles.footer}>
          <p>Used: {data.used}GB</p>
          <p>Unused: {data.available}GB</p>
        </div>
      </div>
    </div>
  );
};
