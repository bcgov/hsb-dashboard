'use client';

import { useFiltered } from '@/store';
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
import { LineChart } from '../lineChart';
import { defaultData } from './defaultData';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface LineChartProps {
  large?: boolean;
}

export const StorageTrendsChart: React.FC<LineChartProps> = ({ large }) => {
  const fileSystemItems = useFiltered((state) => state.fileSystemItems);

  return (
    <LineChart data={defaultData} label="Storage Trends" large={large} showExport exportDisabled />
  );
};
