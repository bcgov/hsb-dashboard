'use client';

import React from 'react';
import styles from './Chart.module.scss';
import { Button } from '@/components/buttons';
import { Line } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
} from 'chart.js';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

interface LineChartProps {
  large?: boolean;
}

export const LineChart: React.FC<LineChartProps> = ({ large }) => {

  // Example data for two lines: storage used and storage allocated
  const data = {
    labels: Array.from(new Array(12), (val, index) => `Month ${index + 1}`),
    datasets: [
      {
        label: 'Total Used in TB',
        data: Array.from(new Array(12), (_, i) => Math.random() * 10 + 50), // Randomly generated data
        borderColor: '#313132',
        backgroundColor: '#313132',
        fill: false,
      },
      {
        label: 'Total Allocated in TB',
        data: Array.from(new Array(12), (_, i) => Math.random() * 10 + 70), // Randomly generated data
        borderColor: '#476E94',
        backgroundColor: '#476E94',
        fill: false,
      },
    ],
  };

  // Configuration for the line chart
  const options = {
    responsive: true,
    maintainAspectRatio: large ? false : true,
    plugins: {
      legend: {
        position: 'bottom' as const,
        align: 'start' as const,
        labels: {
          boxWidth: 18,
          padding: 20,
          color: '#313132',
          font: {
            size: 16,
            fontFamily: 'BCSans',
          },
        },
      },
      title: {
        display: false,
      },
    },
    layout: {
      padding: {
        top: 20,
      }
    },
  };

  return (
    <div className={`${styles.lineChart} ${large ? styles.panelLarge : styles.panel}`}>
      <h1>Storage Trends</h1>
      <div className={styles.chartContainer}>
        <Line data={data} options={options} />
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png">
        Export to Excel
      </Button>
    </div>
  );
};
