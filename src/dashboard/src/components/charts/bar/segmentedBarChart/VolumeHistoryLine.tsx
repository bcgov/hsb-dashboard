'use client';

import { Button, DateRangePicker, LineChart } from '@/components';
import styles from './VolumeHistorySmallMultiples.module.scss';

import { IServerItemListModel } from '@/hooks';
import { useStorageTrendsStore } from '@/store';
import {
  CategoryScale,
  Chart as ChartJS,
  Legend,
  LinearScale,
  LineElement,
  Title,
  Tooltip,
} from 'chart.js';
import moment from 'moment';
import React from 'react';
import { toast } from 'react-toastify';
import { LoadingAnimation } from '../../../loadingAnimation';
import { useFileSystemHistoryItems } from './hooks';
import { useStorageTrendsData } from './useStorageTrendsData';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faLightbulb } from '@fortawesome/free-solid-svg-icons';

ChartJS.register(CategoryScale, LinearScale, LineElement, Title, Tooltip, Legend);

export interface IVolumeHistoryLine {
  serverItem?: IServerItemListModel;
  loading?: boolean;
  dateRange?: string[];
  minColumns?: number;
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: () => void;
}

export const VolumeHistoryLine = ({
  serverItem,
  loading,
  dateRange: initDateRange,
  minColumns = 12,
  showExport,
  exportDisabled,
  onExport,
}: IVolumeHistoryLine) => {
  const getStorageTrends = useStorageTrendsData();
  const dateRange = useStorageTrendsStore((state) => state.dateRangeFileSystemHistoryItems);
  const setDateRange = useStorageTrendsStore((state) => state.setDateRangeFileSystemHistoryItems);
  const { isReady: fileSystemHistoryItemsIsReady, findFileSystemHistoryItems } =
    useFileSystemHistoryItems();

  const [showAbsolute, setShowAbsolute] = React.useState(true);

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

  const data = getStorageTrends(1, dateRange);

  // If absolute, we want to show the raw storage amount used. If proportional, show the percentage.
  if (showAbsolute) {
    data.datasets = data.datasets.filter((d) => d.label?.startsWith('Used'));
  } else {
    data.datasets = data.datasets.filter((d) => d.label?.startsWith('% Used'));
  }

  // Get unique datasets by name
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
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1>
          Storage Trends - {serverItem?.name ?? 'Drive'} Storage (
          {showAbsolute ? 'Absolute Usage' : 'Proportional Usage'})
        </h1>
        <Button onClick={() => setShowAbsolute(!showAbsolute)}>
          {showAbsolute ? 'Show Proportional Usage' : 'Show Absolute Usage'}
        </Button>
      </div>
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
      <div>
        {/* TODO: Make this chart taller. */}
        <LineChart data={data as any} large>
          <div
            style={{ marginTop: '16px', fontSize: '14px', color: '#595959', textAlign: 'center' }}
          >
            <FontAwesomeIcon icon={faLightbulb} style={{ color: '#FCBA19', marginRight: '4px' }} />{' '}
            Tip: You can click the drive name to hide / show that drive in the chart.
          </div>
        </LineChart>
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
