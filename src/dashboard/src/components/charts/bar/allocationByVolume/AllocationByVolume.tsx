'use client';

import { IFileSystemItemModel } from '@/hooks';
import { BarRow, SmallBarChart } from '../smallBar';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';
import styles from '../smallBar/SmallBarChart.module.scss';
import defaultData from './defaultData';

export interface IAllocationByVolumeProps {
  fileSystemItems: IFileSystemItemModel[];
  loading?: boolean;
  onClick?: (fileSystemItem?: IFileSystemItemModel) => void;
}

export const AllocationByVolume = ({
  fileSystemItems,
  loading,
  onClick,
}: IAllocationByVolumeProps) => {
  return (
    <SmallBarChart
      title="Drive Space"
      loading={loading}
      data={{
        ...defaultData,
        datasets: fileSystemItems
          .map<IBarChartRowData<IFileSystemItemModel>>((fsi) => ({
            key: fsi.name,
            label: fsi.name,
            capacity: fsi.capacity,
            available: fsi.availableSpace,
            data: fsi,
          }))
          .sort((a, b) =>
            a.capacity < b.capacity
              ? 1
              : a.capacity > b.capacity
              ? -1
              : a.label < b.label
              ? -1
              : a.label > b.label
              ? 1
              : 0,
          ),
      }}
      exportDisabled={true}
      onExport={() => {}}
    >
      {(data) => {
        return data.datasets.map((os) => (
          <BarRow
            key={os.key}
            label={
              <p>
                {onClick ? (
                  <label className={styles.link} onClick={() => onClick?.(os.data)}>
                    {os.label}
                  </label>
                ) : (
                  <label>{os.label}</label>
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
