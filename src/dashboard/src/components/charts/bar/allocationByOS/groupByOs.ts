import { IOperatingSystemItemModel, IServerItemModel } from '@/hooks';
import { groupBy } from '@/utils';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';

export const groupByOS = (
  serverItems: IServerItemModel[],
  operatingSystemItems: IOperatingSystemItemModel[],
) => {
  const groups = groupBy<IServerItemModel, IBarChartRowData>(
    serverItems,
    (item) => `${item.operatingSystemItemId ?? 'NA'}`,
    (item) => {
      return {
        label: item.name,
        capacity: item.capacity ?? 0,
        available: item.availableSpace ?? 0,
        used: (item.capacity ?? 0) - (item.availableSpace ?? 0),
      };
    },
  );

  const result = Object.keys(groups)
    .map<IBarChartRowData>((key) => {
      const items = groups[key];
      const capacity = items.reduce((result, item) => result + item.capacity, 0);
      const available = items.reduce((result, item) => result + item.available, 0);
      const used = items.reduce((result, item) => result + item.used, 0);
      const label =
        key === 'NA' ? 'NA' : operatingSystemItems.find((os) => os.id === +key)?.name ?? 'NA';

      return { label, capacity, available, used };
    })
    .sort((a, b) => (a.label < b.label ? -1 : a.label > b.label ? 1 : 0));

  return result;
};
