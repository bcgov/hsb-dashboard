'use client';

import { DateRangePicker } from '@/components';
import { useDashboard } from '@/hooks/dashboard';
import { useStorageTrendsStore } from '@/store';
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
import moment from 'moment';
import React from 'react';
import { toast } from 'react-toastify';
import { LineChart } from '../line';
import styles from './StorageTrendsChart.module.scss';
import { useServerHistoryItems, useStorageTrendsData } from './hooks';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

interface LineChartProps {
  /** Minimum number of columns to display */
  minColumns?: number;
  /** If the chart is large. */
  large?: boolean;
  /** Whether the chart data is loading */
  loading?: boolean;
  /** Date range selected for the filter. */
  dateRange?: string[];
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: (startDate?: string, endDate?: string) => void;
}

/**
 * Provides a component to display server storage trends over time.
 * @param param0 Component properties.
 * @returns Component.
 */
export const StorageTrendsChart: React.FC<LineChartProps> = ({
  large,
  loading,
  minColumns,
  dateRange: initDateRange,
  showExport,
  exportDisabled,
  onExport,
}) => {
  const getStorageTrendsData = useStorageTrendsData();
  const dateRange = useStorageTrendsStore((state) => state.dateRangeServerHistoryItems);
  const setDateRange = useStorageTrendsStore((state) => state.setDateRangeServerHistoryItems);
  const { isReady: serverHistoryItemsIsReady, findServerHistoryItems } = useServerHistoryItems();
  const { tenantId, organizationId, operatingSystemItemId, serverItemKey } = useDashboard();

  React.useEffect(() => {
    const values = [
      initDateRange?.length && initDateRange[0]
        ? initDateRange[0]
        : moment().add(-1, 'year').format('YYYY-MM-DD'),
      initDateRange?.length && initDateRange[1] ? initDateRange[1] : '',
    ];
    setDateRange(values);
    // Infinite loop if we use the array instead of individual values.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [initDateRange?.[0], initDateRange?.[1], setDateRange]);

  React.useEffect(() => {
    if (dateRange[0]) {
      // A single server was selected, fetch the history for this server.
      findServerHistoryItems({
        tenantId: tenantId,
        organizationId: organizationId,
        operatingSystemItemId: operatingSystemItemId,
        serviceNowKey: serverItemKey,
        startDate: dateRange[0],
        endDate: dateRange[1] ? dateRange[1] : undefined,
      }).catch((ex) => {
        const error = ex as Error;
        toast.error(error.message);
        console.error(error);
      });
    }
    // Values array will cause infinite loop, we're only interested in when values change.
    // findServerHistoryItems will cause infinite loop.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    findServerHistoryItems,
    tenantId,
    organizationId,
    operatingSystemItemId,
    serverItemKey,
    // eslint-disable-next-line react-hooks/exhaustive-deps
    dateRange[0],
    // eslint-disable-next-line react-hooks/exhaustive-deps
    dateRange[1],
  ]);

  return (
    <LineChart
      data={getStorageTrendsData(minColumns, dateRange)}
      label="Storage Trends"
      loading={loading || !serverHistoryItemsIsReady}
      large={large}
      disclaimer={
        <p className={styles.disclaimer}>*Data shows totals on last available day of each month.</p>
      }
      showExport={showExport}
      exportDisabled={exportDisabled}
      onExport={() => onExport?.(dateRange[0], dateRange[1])}
      filter={
        <div className={styles.date}>
          <DateRangePicker
            values={dateRange}
            onChange={async (values, e) => {
              setDateRange(values);
            }}
            showButton
          />
        </div>
      }
    />
  );
};
