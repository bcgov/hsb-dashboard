'use client';

import { Button } from '@/components/buttons';
import styles from './SegmentedBarChart.module.scss';
import { Bar } from 'react-chartjs-2';
import { defaultData } from './defaultData';

import {
  CategoryScale,
  Chart as ChartJS,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

export const SegmentedBarChart: React.FC = () => {

 const data = defaultData(); 

  const options = {
    scales: {
      x: {
        stacked: true,
      },
      y: {
        stacked: true,
      },
    },
  };

  return (
    <div className={styles.panel}>
      <h1>Storage Trends - Server 1 Drive Storage</h1>
      <div className={styles.chartContainer}>
        <Bar data={data} options={options} />
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png" disabled>
        Export to Excel
      </Button>
    </div>
  );
};
