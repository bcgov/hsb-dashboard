'use client';

import { DateRangePicker } from '@/components';
import { IServerItemListModel } from '@/hooks';
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
  /** An array of server items  */
  serverItems?: IServerItemListModel[];
  /** Whether to show the export button. */
  showExport?: boolean;
  /** Whether the export button is disabled. */
  exportDisabled?: boolean;
  /** Event fires when export button is clicked. */
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
  serverItems = [],
  showExport,
  exportDisabled,
  onExport,
}) => {
  const getStorageTrendsData = useStorageTrendsData();
  const dateRange = useStorageTrendsStore((state) => state.dateRangeServerHistoryItems);
  const setDateRange = useStorageTrendsStore((state) => state.setDateRangeServerHistoryItems);
  const { isReady: serverHistoryItemsIsReady, findServerHistoryItems } = useServerHistoryItems();
  const { tenantId, organizationId, operatingSystemItemId, serverItemKey } = useDashboard();

  const [forceFetch, setForceFetch] = React.useState(0);

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

  const fetch = React.useCallback(
    (startDate?: string, endDate?: string) => {
      if (startDate) {
        // A single server was selected, fetch the history for this server.
        return findServerHistoryItems({
          tenantId: tenantId,
          organizationId: organizationId,
          operatingSystemItemId: operatingSystemItemId,
          serviceNowKey: serverItemKey,
          startDate: startDate,
          endDate: endDate ? endDate : undefined,
        }).catch((ex) => {
          const error = ex as Error;
          toast.error(error.message);
          console.error(error);
        });
      }
    },
    [findServerHistoryItems, operatingSystemItemId, organizationId, serverItemKey, tenantId],
  );

  React.useEffect(() => {
    fetch(dateRange[0], dateRange[1])?.catch(() => {});
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
    forceFetch,
  ]);

  return (
    <LineChart
      data={getStorageTrendsData(minColumns, dateRange, serverItems)}
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
            onChange={(values, e) => {
              if (values[0] !== dateRange[0] || values[1] !== dateRange[1]) setDateRange(values);
              else setForceFetch(Date.now());
            }}
            showButton
          />
        </div>
      }
    />
  );
};
