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
import { LineChart } from '../line';
import styles from './StorageTrendsChart.module.scss';
import { useServerHistoryItems } from './hooks';
import { useStorageTrendsData } from './useStorageTrendsData';

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
}) => {
  const getStorageTrendsData = useStorageTrendsData();
  const dateRange = useStorageTrendsStore((state) => state.dateRangeServerHistoryItems);
  const setDateRange = useStorageTrendsStore((state) => state.setDateRangeServerHistoryItems);
  const { isReady: serverHistoryItemsIsReady, findServerHistoryItems } = useServerHistoryItems();
  const { tenantId, organizationId, operatingSystemItemId, serverItemKey } = useDashboard();

  const values = [
    initDateRange?.length && initDateRange[0]
      ? initDateRange[0]
      : moment().add(-1, 'year').format('YYYY-MM-DD'),
    initDateRange?.length && initDateRange[1] ? initDateRange[1] : '',
  ];

  React.useEffect(() => {
    setDateRange(values);
    // Infinite loop if we use the array instead of individual values.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [values[0], values[1], setDateRange]);

  React.useEffect(() => {
    if (values[0]) {
      // A single server was selected, fetch the history for this server.
      findServerHistoryItems({
        tenantId: tenantId,
        organizationId: organizationId,
        operatingSystemItemId: operatingSystemItemId,
        serviceNowKey: serverItemKey,
        startDate: values[0],
        endDate: values[1] ? values[1] : undefined,
      }).catch((error) => {
        console.error(error);
      });
    }
    // Values array will cause infinite loop, we're only interested in the values.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    findServerHistoryItems,
    tenantId,
    organizationId,
    operatingSystemItemId,
    serverItemKey,
    // eslint-disable-next-line react-hooks/exhaustive-deps
    values[0],
    // eslint-disable-next-line react-hooks/exhaustive-deps
    values[1],
  ]);

  return (
    <LineChart
      data={getStorageTrendsData(minColumns, dateRange)}
      label="Storage Trends"
      loading={loading || !serverHistoryItemsIsReady}
      large={large}
      showExport
      exportDisabled
      filter={
        <div className={styles.date}>
          <DateRangePicker
            values={dateRange}
            onChange={async (values, e) => {
              setDateRange(values);
              await findServerHistoryItems({
                tenantId: tenantId,
                organizationId: organizationId,
                operatingSystemItemId: operatingSystemItemId,
                serviceNowKey: serverItemKey,
                startDate: values[0] ? values[0] : undefined,
                endDate: values[1] ? values[1] : undefined,
              });
            }}
            showButton
          />
        </div>
      }
    />
  );
};
