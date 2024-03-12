'use client';

import { Button } from '@/components';
import {
  CategoryScale,
  ChartData,
  Chart as ChartJS,
  ChartOptions,
  ChartType,
  DefaultDataPoint,
  Legend,
  LineElement,
  LinearScale,
  PointElement,
  Title,
  Tooltip,
} from 'chart.js';
import { Line } from 'react-chartjs-2';
import { LoadingAnimation } from '../../loadingAnimation';
import styles from './LineChart.module.scss';
import { defaultChartOptions } from './defaultChartOptions';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface LineChartProps<TData = DefaultDataPoint<'line'>, TLabel = unknown> {
  label?: string;
  large?: boolean;
  options?: ChartOptions<'line'>;
  filter?: React.ReactNode;
  data: ChartData<'line', TData, TLabel>;
  showExport?: boolean;
  exportDisabled?: boolean;
  loading?: boolean;
}

export const LineChart = <
  TType extends ChartType = ChartType,
  TData = DefaultDataPoint<TType>,
  TLabel = unknown,
>({
  label,
  large,
  data,
  options = defaultChartOptions,
  filter,
  showExport,
  exportDisabled,
  loading,
}: LineChartProps<TData, TLabel>) => {
  return (
    <div className={`${styles.lineChart} ${large ? styles.panelLarge : styles.panel}`}>
      {loading && <LoadingAnimation />}
      {label && <h1>{label}</h1>}
      {filter}
      <div className={styles.chartContainer}>
      <p className={styles.disclaimer}>*Data shows totals on last available day of each month.</p>
        <Line data={data} options={{ ...options, maintainAspectRatio: !large }} />
      </div>
      {showExport && (
        <Button variant="secondary" iconPath="/images/download-icon.png" disabled={exportDisabled}>
          Export to Excel
        </Button>
      )}
    </div>
  );
};
