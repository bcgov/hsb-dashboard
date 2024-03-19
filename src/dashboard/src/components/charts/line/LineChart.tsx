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
  disclaimer?: React.ReactNode;
  data: ChartData<'line', TData, TLabel>;
  loading?: boolean;
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: () => void;
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
  disclaimer,
  loading,
  showExport,
  exportDisabled,
  onExport,
}: LineChartProps<TData, TLabel>) => {
  return (
    <div className={`${styles.lineChart} ${large ? styles.panelLarge : styles.panel}`}>
      {loading && <LoadingAnimation />}
      {label && <h1>{label}</h1>}
      {filter}
      {disclaimer}
      <div className={styles.chartContainer}>
        <Line data={data} options={{ ...options, maintainAspectRatio: !large }} />
      </div>
      {showExport && (
        <Button
          variant="secondary"
          iconPath="/images/download-icon.png"
          disabled={exportDisabled}
          onClick={onExport}
        >
          Export to Excel
        </Button>
      )}
    </div>
  );
};
