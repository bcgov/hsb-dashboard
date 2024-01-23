'use client';

import {
  CategoryScale,
  Chart as ChartJS,
  Legend,
  LineElement,
  LinearScale,
  PointElement,
  Title,
  Tooltip,
} from 'chart.js';
import React from 'react';
import { LineChart } from '../line';
import { useStorageTrends } from './useStorageTrends';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface LineChartProps {
  minColumns?: number;
  large?: boolean;
  loading?: boolean;
  dateRange?: string[];
}

export const StorageTrendsChart: React.FC<LineChartProps> = ({
  large,
  loading,
  minColumns,
  dateRange,
}) => {
  const data = useStorageTrends(minColumns, dateRange);

  return <LineChart data={data} label="Storage Trends" large={large} showExport exportDisabled />;
};
