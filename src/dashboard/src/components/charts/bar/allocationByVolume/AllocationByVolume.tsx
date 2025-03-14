'use client';

import { IFileSystemItemModel, IServerItemListModel, IServerItemModel } from '@/hooks';
import { BarRow, SmallBarChart } from '../smallBar';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';
import styles from '../smallBar/SmallBarChart.module.scss';
import defaultData from './defaultData';

export interface IAllocationByVolumeProps {
  fileSystemItems: IFileSystemItemModel[];
  dashboardServerItem?: IServerItemListModel;
  loading?: boolean;
  onClick?: (fileSystemItem?: IFileSystemItemModel) => void;
}

export const AllocationByVolume = ({
  dashboardServerItem,
  fileSystemItems,
  loading,
  onClick,
}: IAllocationByVolumeProps) => {
  console.log('dashboardServerItem', dashboardServerItem);

  return (
    <SmallBarChart
      title="Drive Space"
      loading={loading}
      data={{
        ...defaultData,
        datasets: fileSystemItems
          .map<IBarChartRowData<IFileSystemItemModel>>((fsi) => {
            return {
              key: fsi.name,
              label: fsi.name,
              capacity: fsi.sizeBytes,
              available: fsi.freeSpaceBytes,
              data: fsi,
            };
          })
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
        return data.datasets.map((fsi) => (
          <BarRow
            key={fsi.key}
            label={
              <>
                {onClick ? (
                  <label className={styles.link} onClick={() => onClick?.(fsi.data)}>
                    {fsi.label}
                  </label>
                ) : (
                  <label className={styles.linkStatic} title={fsi.label}>
                    {fsi.label}
                  </label>
                )}
              </>
            }
            capacity={fsi.capacity}
            available={fsi.available}
            data={fsi.data}
          />
        ));
      }}
    </SmallBarChart>
  );
};
