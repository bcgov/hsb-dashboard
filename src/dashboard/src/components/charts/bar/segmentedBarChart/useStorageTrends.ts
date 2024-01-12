import { useStorageHistoryDateRange } from '@/components/charts/hooks';
import { IFileSystemHistoryItemModel } from '@/hooks';
import { useDashboard } from '@/store';
import { groupBy } from '@/utils';
import { ChartData } from 'chart.js';
import moment from 'moment';
import React from 'react';
import { convertToStorageSize } from './../../../../utils/convertToStorageSize';
import { IVolumeData } from './IVolumeData';

const colorPairs = [
  ['#4D7194', '#86BAEF'],
  ['#E9B84E', '#FFD57B'],
  ['#A9A9A9', '#D7D7D7'],
];

interface IStorageTrendsData extends ChartData<'bar', number[], string> {
  volumes: IVolumeData[];
}

/**
 * Generates line chart data based on the current filtered server history items.
 * @param minColumns Minimum number of columns in the line chard (default = 1).
 * @param maxVolumes Maximum number of mapped volumes that can be displayed (default = 5).
 * @returns Line chart data.
 */
export const useStorageTrends = (
  minColumns: number = 1,
  maxVolumes: number = 4,
): IStorageTrendsData => {
  const groups = useStorageHistoryDateRange<IFileSystemHistoryItemModel>(minColumns);
  const fileSystemHistoryItems = useDashboard((state) => state.fileSystemHistoryItems);

  return React.useMemo(() => {
    const history = fileSystemHistoryItems.filter((item) => item.mediaType === 'fixed');

    // server history is returned for each server, however some servers may lack history.
    // This process needs to group each month.
    const items = history
      .map((item) => {
        const createdOn = moment(item.createdOn);
        const month = '0' + (createdOn.month() + 1);
        return {
          ...item,
          key: `${createdOn.year()}-${month.substring(month.length - 2)}`,
          year: createdOn.year(),
          month: createdOn.month() + 1,
        };
      })
      .reduce((result, item) => {
        const { key } = item;
        (result as any)[key] = (result as any)[key] ?? [];
        (result as any)[key].push(item);
        return result;
      }, {});

    groups.forEach((group) => {
      const values: IFileSystemHistoryItemModel[] = (items as any)[group.key] ?? [];
      group.items = values;
    });

    // Extract the history for each mapped volume / drive.
    const volumeHistory = groupBy<IFileSystemHistoryItemModel, IVolumeData>(
      history,
      (item) => item.serviceNowKey,
      (item) => ({
        serviceNowKey: item.serviceNowKey,
        name: item.name,
        capacity: item.capacity,
        availableSpace: item.availableSpace,
        createdOn: item.createdOn,
      }),
    );
    // Take the last item in each sub-array, it should be the most recent entry.
    const volumes = Object.values(volumeHistory)
      .map((item) => item[item.length - 1])
      .sort((a, b) => (a.capacity < b.capacity ? 1 : a.capacity > b.capacity ? -1 : 0));

    // If there is more than the max, we actually only show one less than the max.
    // We do this because we need space to provide a placeholder informing the user of additional volumes.
    const actualMaxVolumes = maxVolumes < volumes.length ? maxVolumes - 1 : maxVolumes;

    return {
      labels: groups.map((i) => i.label),
      volumes: volumes,
      datasets: volumes
        .slice(0, actualMaxVolumes)
        .map((volume, index) => {
          // Get color pair based on the current drive
          const cIndex =
            index === 0
              ? 0
              : index === 1
              ? 1
              : index === 2
              ? 2
              : index % 3 === 0
              ? 0
              : index % 2 === 0
              ? 1
              : 2;
          const colors = colorPairs[cIndex];

          // Merge the data for each volume into each group.
          // There should only ever be one record per volume for each month.
          // We use the last record in the array for each month.
          const groupData = groups.map((group) => {
            const items = group.items.filter((i) => i.serviceNowKey === volume.serviceNowKey);
            const capacity = convertToStorageSize<number>(
              items[items.length - 1].capacity,
              'MB',
              'GB',
              { type: 'number' },
            );
            const available = convertToStorageSize<number>(
              items[items.length - 1].availableSpace,
              'MB',
              'GB',
              { type: 'number' },
            );
            const used = capacity - available;
            return {
              capacity,
              available,
              used,
            };
          });

          // This results in an array of mapped volumes, which contains an array of two datasets.
          // One dataset is an array of used space grouped by month.
          // The second dataset is an array of unused space grouped by month.
          return [
            {
              label: `Used ${volume.name} (${convertToStorageSize(volume.capacity, 'MB', 'GB', {
                formula: (value) => Number(value.toFixed(1)),
              })})`,
              name: volume.name,
              capacity: convertToStorageSize(volume.capacity, 'MB', 'GB', {
                formula: (value) => Number(value.toFixed(1)),
              }),
              data: groupData.map((group) => group.used), // Record of the volume data for each group (month).
              backgroundColor: colors[0],
              stack: `Stack ${index - 1}`,
            },
            {
              label: `Unused ${volume.name} (${convertToStorageSize(volume.capacity, 'MB', 'GB', {
                formula: (value) => Number(value.toFixed(1)),
              })})`,
              name: volume.name,
              capacity: convertToStorageSize(volume.capacity, 'MB', 'GB', {
                formula: (value) => Number(value.toFixed(1)),
              }),
              data: groupData.map((group) => group.available), // Record of the volume data for each group (month).
              backgroundColor: colors[1],
              stack: `Stack ${index - 1}`,
            },
          ];
        })
        .reduce((result, volume) => {
          // Pull the datasets out of each volume and flatten the array.
          // Now there will be a dataset containing two records for each volume.
          result.push(volume[0], volume[1]);
          return result;
        }, []),
    };
  }, [fileSystemHistoryItems, groups, maxVolumes]);
};
