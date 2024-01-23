import { calcMonthsBetween } from '@/utils';
import moment from 'moment';

export interface IDateRangeStorageHistoryData<T> {
  key: string;
  label: string;
  items: T[];
  capacity: number;
  availableSpace: number;
  usedSpace: number;
}

/**
 * Generate a column for each month, with a minimum number of columns.
 * @param minColumns Minimum number of columns
 * @returns An array of months to contain history for each month.
 */
export const useStorageHistoryDateRange = <T>(minColumns: number = 1, dateRange: string[] = []) => {
  const now = moment();
  const start = dateRange[0]
    ? moment(dateRange[0])
    : moment(new Date(now.year(), now.month(), 1)).add(-1 * minColumns, 'months');
  const end = dateRange[1] ? moment(dateRange[1]) : moment(Date.now());

  const numberOfMonths = calcMonthsBetween(start.toDate(), end.toDate());
  const minPoints = numberOfMonths > minColumns ? numberOfMonths : minColumns;

  const endSafeDate = moment(new Date(end.year(), end.month(), 1));
  return Array.from(new Array(minPoints), (_, index) => {
    const date = endSafeDate.clone().add(-1 * (minPoints - 1 - index), 'months');
    const month = '0' + (date.month() + 1);
    const result: {
      key: string;
      label: string;
      items: T[];
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
};
