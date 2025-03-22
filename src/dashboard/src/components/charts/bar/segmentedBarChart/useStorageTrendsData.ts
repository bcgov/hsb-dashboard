import { generateStorageHistoryForDateRange } from '@/components';
import { IFileSystemHistoryItemModel } from '@/hooks';
import { useStorageTrendsStore } from '@/store';
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

const borderDash = [
  [0, 0],
  [5, 5],
  [10, 10],
  [15, 15],
  [20, 20],
];

interface IStorageTrendsData extends ChartData<'bar', number[], string> {
  volumes: IVolumeData[];
}

/**
 * Generates line chart data based on the current filtered server history items.
 * @returns Line chart data.
 */
export const useStorageTrendsData = (): ((
  minColumns?: number,
  dateRange?: string[],
) => IStorageTrendsData) => {
  const fileSystemHistoryItems = useStorageTrendsStore((state) => state.fileSystemHistoryItems);

  /**
   *
   * @param minColumns Minimum number of columns in the line chard (default = 1).
   * @param dateRange Array containing start and end dates.
   */
  return React.useCallback(
    (minColumns: number = 1, dateRange: string[] = []) => {
      const groups = generateStorageHistoryForDateRange<IFileSystemHistoryItemModel>(
        minColumns,
        dateRange,
      );

      const history = fileSystemHistoryItems.filter((item) => item.mediaType === 'fixed');

      console.log(
        'history',
        history.map((h) => ({ name: h.name, createdOn: h.createdOn })),
      );

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

      console.log('items', items);

      groups.forEach((group) => {
        const values: IFileSystemHistoryItemModel[] = (items as any)[group.key] ?? [];
        group.items = values;
      });

      console.log('groups', groups);

      // Extract the history for each mapped volume / drive.
      const volumeHistory = groupBy<IFileSystemHistoryItemModel, IVolumeData>(
        history,
        (item) => item.name,
        (item) => {
          return {
            serviceNowKey: item.serviceNowKey,
            name: item.name,
            capacity: item.sizeBytes,
            availableSpace: item.freeSpaceBytes,
            createdOn: item.createdOn,
            createdYYMM: moment(item.createdOn).format('YYYY-MM'),
          };
        },
      );

      // In some cases, drives have had their key change, even if they're the same drive (judged by
      // their name). This might give us two entries for the same month. We need to find the most
      // recent of these entries, and discard the other.
      Object.keys(volumeHistory).forEach((key) => {
        const items = volumeHistory[key];

        // First, sort the items by createdOn date.
        if (items.length > 1) {
          const sorted = items.sort((a, b) => (a.createdOn > b.createdOn ? 1 : -1));
          volumeHistory[key] = sorted;
        }

        // Next, remove any duplicates based on the year-month of the createdOn date. First, we
        // map by year-month, then we take the last item in each sub-array.
        const mapped = groupBy<IVolumeData, IVolumeData>(
          items,
          (item) => item.createdYYMM,
          (item) => item,
        );
        volumeHistory[key] = Object.values(mapped).map((item) => item[item.length - 1]);
      });

      console.log('volumeHistory', volumeHistory);

      // Take the last item in each sub-array, it should be the most recent entry.
      const volumes = Object.values(volumeHistory)
        .map((volumeData) => volumeData[volumeData.length - 1])
        .sort((a, b) => (a.capacity < b.capacity ? 1 : a.capacity > b.capacity ? -1 : 0));
      console.log('volumes', volumes);

      const dataResult = {
        labels: groups.map((i) => i.label),
        volumes: volumes,
        datasets: volumes
          .map((volume, index) => {
            // Get color pair based on the current drive
            const colors = colorPairs[index % colorPairs.length];

            const data = groups.map((group) => {
              const item = volumeHistory[volume.name].find((v) => v.createdYYMM === group.key);
              return item ?? { capacity: 0, availableSpace: 0 };
            });

            // Merge the data for each volume into each group.
            // There should only ever be one record per volume for each month.
            // We use the last record in the array for each month.
            const groupData = data.map((monthData) => {
              const capacity = convertToStorageSize<number>(monthData.capacity || 0, 'B', 'GB', {
                type: 'number',
              });
              const available = convertToStorageSize<number>(
                monthData.availableSpace || 0,
                'B',
                'GB',
                {
                  type: 'number',
                },
              );
              const used = capacity - available;
              return {
                capacity,
                available,
                used,
              };
            });

            return [
              {
                label: `Used ${volume.name} (Capacity: ${convertToStorageSize(
                  volume.capacity,
                  'B',
                  'GB',
                  {
                    formula: (value) => Number(value.toFixed(1)),
                  },
                )})`,
                name: volume.name,
                capacity: convertToStorageSize(volume.capacity, 'B', 'GB', {
                  formula: (value) => Number(value.toFixed(1)),
                }),
                data: groupData.map((group) => group.used), // Record of the volume data for each group (month).
                backgroundColor: colors[0],
                borderColor: colors[0],
                borderWidth: 3,
                borderDash: borderDash[Math.floor(index / colorPairs.length) % borderDash.length],
                stack: `Stack ${index - 1}`,
              },
              {
                label: `Unused ${volume.name} (${convertToStorageSize(volume.capacity, 'B', 'GB', {
                  formula: (value) => Number(value.toFixed(1)),
                })})`,
                name: volume.name,
                capacity: convertToStorageSize(volume.capacity, 'B', 'GB', {
                  formula: (value) => Number(value.toFixed(1)),
                }),
                data: groupData.map((group) => group.available), // Record of the volume data for each group (month).
                backgroundColor: colors[1],
                borderColor: colors[1],
                borderWidth: 3,
                borderDash: borderDash[Math.floor(index / colorPairs.length) % borderDash.length],
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

      console.log('dataResult', dataResult);

      return dataResult;
    },
    [fileSystemHistoryItems],
  );
};
