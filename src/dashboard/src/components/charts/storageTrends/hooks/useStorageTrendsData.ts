import { IServerHistoryItemModel, IServerItemListModel } from '@/hooks';
import { useStorageTrendsStore } from '@/store';
import { convertToStorageSize } from '@/utils';
import { ChartData } from 'chart.js';
import moment from 'moment';
import React from 'react';
import { generateStorageHistoryForDateRange } from '../../utils';
import { convertStorageSize } from './../../../../utils/convertToStorageSize';

/**
 * Generates line chart data based on the current filtered server history items.
 * @param minColumns Minimum number of columns in the line chard (default = 1).
 * @returns Line chart data.
 */
export const useStorageTrendsData = (): ((
  minColumns?: number,
  dateRange?: string[],
  serverItems?: IServerItemListModel[],
) => ChartData<'line', number[], string>) => {
  const serverHistoryItems = useStorageTrendsStore((state) => state.serverHistoryItems);

  return React.useCallback(
    (
      minColumns: number = 1,
      dateRange: string[] = [],
      serverItems: IServerItemListModel[] = [],
    ) => {
      const groups = generateStorageHistoryForDateRange(minColumns, dateRange);

      // Determine the output size type to use based on the current total storage of the selected servers.
      const storageSizeType = convertStorageSize(
        serverItems.map((i) => i.capacity ?? 0).reduce((a, b) => a + b, 0),
        'B',
        'TB',
      ).type;

      // server history is returned for each server, however some servers may lack history.
      // This process needs to group each month.
      const items = serverHistoryItems
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
        const values: IServerHistoryItemModel[] = (items as any)[group.key] ?? [];
        group.items = values;
        group.capacity = convertToStorageSize<number>(
          values
            .map((i) => i.capacity)
            .reduce((result, value) => (result ?? 0) + (value ?? 0), 0) ?? 0,
          'B',
          storageSizeType,
          { type: 'number' },
        );
        group.availableSpace = convertToStorageSize<number>(
          values
            .map((i) => i.availableSpace)
            .reduce((result, value) => (result ?? 0) + (value ?? 0), 0) ?? 0,
          'B',
          storageSizeType,
          { type: 'number' },
        );
        group.usedSpace = group.capacity - group.availableSpace;
      });

      return {
        labels: groups.map((i) => i.label),
        datasets: [
          {
            label: `Total Used in ${storageSizeType}`,
            data: groups.map((i) => i.usedSpace),
            borderColor: '#313132',
            backgroundColor: '#313132',
            fill: false,
          },
          {
            label: `Total Allocated in ${storageSizeType}`,
            data: groups.map((i) => i.capacity),
            borderColor: '#476E94',
            backgroundColor: '#476E94',
            fill: false,
          },
        ],
      };
    },
    [serverHistoryItems],
  );
};
