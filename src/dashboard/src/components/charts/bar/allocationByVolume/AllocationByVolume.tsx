'use client';

import { IFileSystemItemModel, IServerItemListModel, IServerItemModel } from '@/hooks';
import { BarRow, SmallBarChart } from '../smallBar';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';
import styles from '../smallBar/SmallBarChart.module.scss';
import defaultData from './defaultData';
import React from 'react';
import { Checkbox } from '@/components/forms';
import { useDashboardStore } from '@/store';

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
  const { onlyShowSAN, setOnlyShowSAN } = useDashboardStore();

  return (
    <SmallBarChart
      title="Drive Space"
      loading={loading}
      topRightContent={
        <div
          style={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'flex-end',
          }}
        >
          <Checkbox
            checked={onlyShowSAN}
            onChange={(e) => {
              setOnlyShowSAN(e.target.checked);
            }}
            label={'Only show SAN'}
          />
        </div>
      }
      data={{
        ...defaultData,
        datasets: fileSystemItems
          .map<IBarChartRowData<IFileSystemItemModel>>((fsi) => ({
            key: fsi.serviceNowKey,
            label: fsi.name,
            capacity: fsi.sizeBytes,
            available: fsi.freeSpaceBytes,
            data: fsi,
          }))
          .filter((fsi) => {
            if (onlyShowSAN) {
              return fsi.data?.isSAN;
            }
            return true;
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
        return data.datasets.map((fsi) => {
          return (
            <BarRow
              key={fsi.key}
              label={
                <>
                  {onClick ? (
                    <label
                      style={{ fontSize: '14px' }}
                      className={styles.link}
                      onClick={() => onClick?.(fsi.data)}
                    >
                      {fsi.label}
                    </label>
                  ) : (
                    <label
                      style={{ fontSize: '14px' }}
                      className={styles.linkStatic}
                      title={fsi.label}
                    >
                      {fsi.label}
                    </label>
                  )}
                </>
              }
              data={fsi.data}
              capacity={fsi.capacity}
              available={fsi.available}
            />
          );
        });
      }}
    </SmallBarChart>
  );
};
