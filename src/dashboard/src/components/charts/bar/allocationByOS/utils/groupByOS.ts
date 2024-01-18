import { IOperatingSystemItemModel, IServerItemModel } from '@/hooks';
import { groupBy } from '@/utils';
import { IBarChartRowData } from '../../smallBar/IBarChartRowData';

export const groupByOS = (
  serverItems: IServerItemModel[],
  operatingSystemItems: IOperatingSystemItemModel[],
) => {
  const groups = groupBy<IServerItemModel, any>(
    serverItems,
    (item) => `${item.operatingSystemItemId ?? 'NA'}`,
    (item) => {
      return {
        key: `${item.operatingSystemItemId ?? 'NA'}`,
        label: item.name.trim(),
        capacity: item.capacity ?? 0,
        available: item.availableSpace ?? 0,
      };
    },
  );

  const result = Object.keys(groups)
    .map<IBarChartRowData<IOperatingSystemItemModel | undefined>>((key) => {
      const items = groups[key];
      const capacity = items.reduce((result, item) => result + item.capacity, 0);
      const available = items.reduce((result, item) => result + item.available, 0);
      const label =
        key === 'NA' ? 'NA' : operatingSystemItems.find((os) => os.id === +key)?.name ?? 'NA';
      const os = operatingSystemItems.find((os) => os.id == +key);

      return { key, label, capacity, available, data: os };
    })
    .sort((a, b) => (a.label < b.label ? -1 : a.label > b.label ? 1 : 0));

  return result;
};
