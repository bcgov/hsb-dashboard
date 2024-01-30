'use client';

import { IOperatingSystemItemModel, IServerItemModel } from '@/hooks';
import { BarRow, SmallBarChart } from '../smallBar';
import styles from '../smallBar/SmallBarChart.module.scss';
import defaultData from './defaultData';
import { groupByOS } from './utils';

export interface IAllocationByOSProps {
  serverItems: IServerItemModel[];
  operatingSystemItems: IOperatingSystemItemModel[];
  loading?: boolean;
  onClick?: (operatingSystemItem?: IOperatingSystemItemModel) => void;
}

export const AllocationByOS = ({
  serverItems,
  operatingSystemItems,
  loading,
  onClick,
}: IAllocationByOSProps) => {
  return (
    <SmallBarChart
      title="Allocation by OS"
      data={{ ...defaultData, datasets: groupByOS(serverItems, operatingSystemItems) }}
      exportDisabled={true}
      onExport={() => {}}
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
