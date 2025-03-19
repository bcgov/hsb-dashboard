'use client';

import { Button, DateRangePicker } from '@/components';
import { Bar } from 'react-chartjs-2';
import styles from './VolumeHistorySmallMultiples.module.scss';

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
import { join } from 'path';

ChartJS.register(CategoryScale, BarElement, Title, Tooltip, Legend);

export interface IVolumeHistorySmallMultiplesBar {
  serverItem?: IServerItemListModel;
  maxVolumes?: number;
  loading?: boolean;
  dateRange?: string[];
  minColumns?: number;
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: () => void;
}

export const VolumeHistorySmallMultiplesBar = ({
  serverItem,
  maxVolumes = 10,
  loading,
  dateRange: initDateRange,
  minColumns = 12,
  showExport,
  exportDisabled,
  onExport,
}: IVolumeHistorySmallMultiplesBar) => {
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
      <div className={styles.chartContainer}>
        {data.volumes.sort().map((volume) => {
          const dataToShow = (data.datasets as any[]).filter(
            (dataset) => dataset.name === volume.name,
          );
          return (
            <div
              key={volume.serviceNowKey}
              style={{
                marginTop: '10px',
                width: '100%',
                borderLeft: '5px solid #eee',
                display: 'flex',
                alignItems: 'center',
              }}
            >
              <div style={{ width: '300px' }}>
                <span style={{ fontWeight: 'bold' }}>{volume.name}</span>
                <br />
                {volume.capacity}
              </div>
              {/* <pre>{JSON.stringify(dataToShow, null, 2)}</pre> */}
              <div style={{ display: 'flex', padding: '5px' }}>
                {data.labels?.map((label, index) => {
                  const usedAmount = dataToShow[index]?.usedAmount ?? 0;
                  return (
                    <div style={{ fontWeight: '600', borderRight: 'solid 1px white' }} key={label}>
                      <div style={{ padding: '5px' }}>
                        <span style={{ fontWeight: 'bold' }}>{label}</span>
                        <br />
                      </div>
                      <div
                        style={{
                          height: `${100}px`,
                          backgroundColor: '#003366',
                        }}
                      >
                        <div
                          style={{ height: `${Math.random() * 100}%`, backgroundColor: '#eee' }}
                        ></div>
                      </div>
                    </div>
                  );
                })}
              </div>
            </div>
          );
        })}
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
