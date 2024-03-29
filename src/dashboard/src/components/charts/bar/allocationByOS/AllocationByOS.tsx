'use client';

import { IOperatingSystemItemListModel, IServerItemListModel } from '@/hooks';
import { BarRow, SmallBarChart } from '../smallBar';
import styles from '../smallBar/SmallBarChart.module.scss';
import defaultData from './defaultData';
import { groupByOS } from './utils';

export interface IAllocationByOSProps {
  serverItems: IServerItemListModel[];
  operatingSystemItems: IOperatingSystemItemListModel[];
  loading?: boolean;
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: () => void;
  onClick?: (operatingSystemItem?: IOperatingSystemItemListModel) => void;
}

export const AllocationByOS = ({
  serverItems,
  operatingSystemItems,
  loading,
  showExport,
  exportDisabled,
  onExport,
  onClick,
}: IAllocationByOSProps) => {
  return (
    <SmallBarChart
      title="Allocation by OS"
      data={{ ...defaultData, datasets: groupByOS(serverItems, operatingSystemItems) }}
      showExport={showExport}
      exportDisabled={exportDisabled}
      onExport={onExport}
      loading={loading}
    >
      {(data) => {
        return data.datasets.map((os) => (
          <BarRow
            key={os.key}
            label={
              <p>
                {onClick && os.data ? (
                  <label className={styles.link} onClick={() => onClick?.(os.data)}>
                    {os.label}
                  </label>
                ) : (
                  os.label
                )}
              </p>
            }
            capacity={os.capacity}
            available={os.available}
          />
        ));
      }}
    </SmallBarChart>
  );
};
