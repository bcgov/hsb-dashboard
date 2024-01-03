'use client';

import { ArcElement, Chart as ChartJS, Tooltip } from 'chart.js';
import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import styles from './DonutChart.module.scss';
// import { defaultData } from './defaultData';

ChartJS.register(ArcElement, Tooltip);

export const DonutChart: React.FC = () => {
  const totalSpaceGB = 100;
  const usedSpaceGB = 75;
  const availableSpaceGB = totalSpaceGB - usedSpaceGB;
  const usedPercentage = (usedSpaceGB / totalSpaceGB) * 100

  const defaultData = {
    space: totalSpaceGB,
    used: usedSpaceGB,
    available: availableSpaceGB,

    labels: ['Used', 'Unused'],
    datasets: [
      {
        data: [usedSpaceGB, availableSpaceGB], // Data for 'Used' and 'Unused'
        backgroundColor: ['#DF9901', '#FFECC2'], // Colors for 'Used' and 'Unused'
        borderColor: ['#DF9901', '#FFECC2'], // Border colors for 'Used' and 'Unused'
        borderWidth: 1,
      }
    ]
  };

  return (
    <div className={styles.panel}>
      <h1>Total Storage Allocation</h1>
      <div className={styles.chartContainer}>
        <div className={styles.chart}>
          <Doughnut
            data={defaultData}
            options={{
              rotation: 180,
              circumference: 360,
              cutout: '85%',
              plugins: {
                legend: {
                  display: false,
                },
                tooltip: {
                  callbacks: {
                    label: function(context: { dataIndex: any; parsed: any; }) {
                      let labelIndex = context.dataIndex;
                      let label = defaultData.labels[labelIndex] || '';
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
        <p className={styles.total}><span>{defaultData.usedPercentage.toFixed(0)}%</span>Used</p>
        <p>Total: {defaultData.space}GB</p>
        <div className={styles.footer}>
            <p>Used: {defaultData.used}GB</p> 
            <p>Unused: {defaultData.available}GB</p>
        </div> 
      </div>
    </div>
  );
};
