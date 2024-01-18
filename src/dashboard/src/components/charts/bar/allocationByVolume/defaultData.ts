import { IFileSystemItemModel } from '@/hooks';
import { IBarChartData } from '../smallBar/IBarChartData';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';

const defaultData: IBarChartData<IBarChartRowData<IFileSystemItemModel>> = {
  labels: ['Drive', 'Allocated', 'Used', 'Unused', 'Percentage Used'],
  datasets: [
    {
      key: 'Drive 1',
      label: 'Drive 1',
      capacity: 12,
      available: 4.8,
    },
    {
      key: 'Drive 2',
      label: 'Drive 2',
      capacity: 12,
      available: 1.6,
    },
    {
      key: 'Drive 3',
      label: 'Drive 3',
      capacity: 12,
      available: 4,
    },
    {
      key: 'Drive 4',
      label: 'Drive 4',
      capacity: 12,
      available: 1,
    },
    {
      key: 'Drive 5',
      label: 'Drive 5',
      capacity: 12,
      available: 3.2,
    },
    {
      key: 'Drive 6',
      label: 'Drive 6',
      capacity: 12,
      available: 0.5,
    },
  ],
};

export default defaultData;
