import { IServerHistoryItemModel } from '@/hooks';
import { useDashboard } from '@/store';
import { calcMonthsBetween, convertToStorageSize } from '@/utils';
import { ChartData } from 'chart.js';
import moment from 'moment';

/**
 * Generates line chart data based on the current filtered server history items.
 * @param minColumns Minimum number of columns in the line chard (default = 12).
 * @returns Line chart data.
 */
export const useStorageTrends = (minColumns: number = 1): ChartData<'line', number[], string> => {
  const dateRange = useDashboard((state) => state.dateRange);
  const serverHistoryItems = useDashboard((state) => state.serverHistoryItems);

  const now = moment();
  const start = dateRange[0]
    ? moment(dateRange[0])
    : moment(new Date(now.year(), now.month(), 1)).add(-1 * minColumns, 'months');
  const end = dateRange[1] ? moment(dateRange[1]) : moment(Date.now());

  const numberOfMonths = calcMonthsBetween(start.toDate(), end.toDate());
  const minPoints = numberOfMonths > minColumns ? numberOfMonths : minColumns;

  const endSafeDate = moment(new Date(end.year(), end.month(), 1));
  const groups = Array.from(new Array(minPoints), (_, index) => {
    const date = endSafeDate.clone().add(-1 * (minPoints - 1 - index), 'months');
    const month = '0' + (date.month() + 1);
    const result: {
      key: string;
      label: string;
      items: IServerHistoryItemModel[];
      capacity: number;
      availableSpace: number;
      usedSpace: number;
    } = {
      key: `${date.year()}-${month.substring(month.length - 2)}`,
      label: `${date.format('MMM')} ${date.format('YYYY')}`,
      items: [],
      capacity: 0,
      availableSpace: 0,
      usedSpace: 0,
    };
    return result;
  });

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
      values.map((i) => i.capacity).reduce((result, value) => (result ?? 0) + (value ?? 0), 0) ?? 0,
      'MB',
      'TB',
      { type: 'number' },
    );
    group.availableSpace = convertToStorageSize<number>(
      values
        .map((i) => i.availableSpace)
        .reduce((result, value) => (result ?? 0) + (value ?? 0), 0) ?? 0,
      'MB',
      'TB',
      { type: 'number' },
    );
    group.usedSpace = group.capacity - group.availableSpace;
  });

  return {
    labels: groups.map((i) => i.label),
    datasets: [
      {
        label: 'Total Used in TB',
        data: groups.map((i) => i.usedSpace),
        borderColor: '#313132',
        backgroundColor: '#313132',
        fill: false,
      },
      {
        label: 'Total Allocated in TB',
        data: groups.map((i) => i.capacity),
        borderColor: '#476E94',
        backgroundColor: '#476E94',
        fill: false,
      },
    ],
  };
};
