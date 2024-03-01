'use client';

import { Button, DateRangePicker } from '@/components';
import { Bar } from 'react-chartjs-2';
import styles from './SegmentedBarChart.module.scss';

import { IServerItemListModel } from '@/hooks';
import { useStorageTrendsStore } from '@/store';
import { BarElement, CategoryScale, Chart as ChartJS, Legend, Title, Tooltip } from 'chart.js';
import moment from 'moment';
import React from 'react';
import { toast } from 'react-toastify';
import { LoadingAnimation } from '../../../loadingAnimation';
import { defaultOptions } from './defaultOptions';
import { useFileSystemHistoryItems } from './hooks';
import { useStorageTrendsData } from './useStorageTrendsData';
import { extractVolumeName } from './utils';

ChartJS.register(CategoryScale, BarElement, Title, Tooltip, Legend);

export interface ISegmentedBarChart {
  serverItem?: IServerItemListModel;
  maxVolumes?: number;
  loading?: boolean;
  dateRange?: string[];
  minColumns?: number;
}

export const SegmentedBarChart = ({
  serverItem,
  maxVolumes = 4,
  loading,
  dateRange: initDateRange,
  minColumns = 12,
}: ISegmentedBarChart) => {
  const getStorageTrends = useStorageTrendsData();
  const dateRange = useStorageTrendsStore((state) => state.dateRangeFileSystemHistoryItems);
  const setDateRange = useStorageTrendsStore((state) => state.setDateRangeFileSystemHistoryItems);
  const { isReady: fileSystemHistoryItemsIsReady, findFileSystemHistoryItems } =
    useFileSystemHistoryItems();

  const now = moment();
  const values = [
    initDateRange?.length && initDateRange[0]
      ? initDateRange[0]
      : moment(new Date(now.year(), now.month(), 1))
          .add(-1 * minColumns, 'months')
          .format('YYYY-MM-DD'),
    initDateRange?.length && initDateRange[1] ? initDateRange[1] : '',
  ];

  React.useEffect(() => {
    setDateRange(values);
    // Infinite loop if we use the array instead of individual values.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [values[0], values[1], setDateRange]);

  const data = getStorageTrends(1, maxVolumes, dateRange);

  React.useEffect(() => {
    if (serverItem) {
      // A single server was selected, fetch the history for this server.
      findFileSystemHistoryItems({
        serverItemServiceNowKey: serverItem.serviceNowKey,
        startDate: values[0] ? values[0] : undefined,
        endDate: values[1] ? values[1] : undefined,
      }).catch((ex) => {
        const error = ex as Error;
        toast.error(error.message);
        console.error(error);
      });
    }
    // Values array will cause infinite loop, we're only interested in the values.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [findFileSystemHistoryItems, serverItem, values[0], values[1]]);

  const CustomLegend = React.useMemo(
    () => (
      <div className={styles.customLegend}>
        {data.datasets
          .filter((_, i) => i % 2 === 0)
          .map((dataset, index) => (
            <div key={index} className={styles.legend}>
              <div className={styles.legendColors}>
                <span style={{ backgroundColor: `${dataset.backgroundColor}` }} />
                <span
                  style={{ backgroundColor: `${data.datasets[index * 2 + 1].backgroundColor}` }}
                />
              </div>
              <div className={styles.legendLabel}>
                <p>{extractVolumeName((dataset as any).name)}</p>
                <p>{(dataset as any).capacity}</p>
              </div>
            </div>
          ))}
        {data.volumes.length * 2 > data.datasets.length && (
          <div>
            <div className={styles.legendColors}>{data.volumes.length} volumes</div>
          </div>
        )}
      </div>
    ),
    [data.datasets, data.volumes.length],
  );

  return (
    <div className={styles.panel}>
      {(loading || !fileSystemHistoryItemsIsReady) && <LoadingAnimation />}
      <h1>Storage Trends - {serverItem?.name ?? 'Drive'} Storage</h1>
      <div className={styles.date}>
        <DateRangePicker
          showButton
          values={dateRange}
          onChange={async (values, e) => {
            setDateRange(values);
            try {
              await findFileSystemHistoryItems({
                serverItemServiceNowKey: serverItem?.serviceNowKey,
                startDate: values[0] ? values[0] : undefined,
                endDate: values[1] ? values[1] : undefined,
              });
            } catch (ex) {
              const error = ex as Error;
              toast.error(error.message);
              console.error(error);
            }
          }}
        />
      </div>
      {CustomLegend}
      <div className={styles.chartContainer}>
        <Bar data={data} options={defaultOptions} />
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png" disabled>
        Export to Excel
      </Button>
    </div>
  );
};
