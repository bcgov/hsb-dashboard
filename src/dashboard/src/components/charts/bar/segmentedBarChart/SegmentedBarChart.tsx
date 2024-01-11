'use client';

import { Button } from '@/components/buttons';
import styles from './SegmentedBarChart.module.scss';
import { Bar } from 'react-chartjs-2';
import { defaultData } from './defaultData';

import {
  CategoryScale,
  Chart as ChartJS,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

ChartJS.register(CategoryScale, BarElement, Title, Tooltip, Legend);

const options = {
  scales: {
    x: {
      stacked: true,
    },
    y: {
      stacked: true,
    },
  },
  plugins: {
    legend: {
      display: false
    }
  },
};

export const SegmentedBarChart: React.FC = () => {
  const numDrives = 3;
  const labelsArray = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];

  const data = defaultData(numDrives, labelsArray);

  const CustomLegend = () => (
    <div className={styles.customLegend}>
      {data.datasets.filter((_, i) => i % 2 === 0).map((dataset, index) => (
        <div key={index}>
          <div className={styles.legendColors}>
            <span style={{ backgroundColor: dataset.backgroundColor }} />
            <span style={{ backgroundColor: data.datasets[index * 2 + 1].backgroundColor }} />
          </div>
          <p className={styles.legendLabel}>{`Drive ${index + 1}`}</p>
        </div>
      ))}
    </div>
  );

  return (
    <div className={styles.panel}>
      <h1>Storage Trends - Drive Storage</h1>
      <CustomLegend />
      <div className={styles.chartContainer}>
        <Bar data={data} options={options} />
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png" disabled>
        Export to Excel
      </Button>
    </div>
  );
};
