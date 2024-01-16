'use client';

import { Button } from '@/components/buttons';
import { Bar } from 'react-chartjs-2';
import styles from './SegmentedBarChart.module.scss';

import { IServerItemModel } from '@/hooks';
import { useDashboardFileSystemHistoryItems } from '@/hooks/dashboard';
import { BarElement, CategoryScale, Chart as ChartJS, Legend, Title, Tooltip } from 'chart.js';
import React from 'react';
import { defaultOptions } from './defaultOptions';
import { useStorageTrends } from './useStorageTrends';
import { extractVolumeName } from './utils';

ChartJS.register(CategoryScale, BarElement, Title, Tooltip, Legend);

export interface ISegmentedBarChart {
  serverItem: IServerItemModel;
  maxVolumes?: number;
  loading?: boolean;
}

export const SegmentedBarChart = ({ serverItem, maxVolumes = 4, loading }: ISegmentedBarChart) => {
  const { findFileSystemHistoryItems } = useDashboardFileSystemHistoryItems();

  const data = useStorageTrends(1, maxVolumes);

  React.useEffect(() => {
    if (serverItem) {
      // A single server was selected, fetch the history for this server.
      findFileSystemHistoryItems({ serverItemServiceNowKey: serverItem.serviceNowKey }).catch(
        () => {},
      );
    }
  }, [findFileSystemHistoryItems, serverItem]);

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
      <h1>Storage Trends - {serverItem?.name ?? 'Drive'} Storage</h1>
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
