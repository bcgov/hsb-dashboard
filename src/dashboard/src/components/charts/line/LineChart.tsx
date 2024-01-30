'use client';

import { Button, DateRangePicker } from '@/components';
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
import styles from './LineChart.module.scss';
import { defaultChartOptions } from './defaultChartOptions';
import { LoadingAnimation } from '../loadingAnimation';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface LineChartProps<TData = DefaultDataPoint<'line'>, TLabel = unknown> {
  label?: string;
  large?: boolean;
  options?: ChartOptions<'line'>;
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
  showExport,
  exportDisabled,
  loading,
}: LineChartProps<TData, TLabel>) => {
  return (
    <div className={`${styles.lineChart} ${large ? styles.panelLarge : styles.panel}`}>
      {loading && <LoadingAnimation />}
      {label && <h1>{label}</h1>}
      <div className={styles.date}>
        <DateRangePicker />
      </div>
      <div className={styles.chartContainer}>
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
