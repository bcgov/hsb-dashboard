import { IBarChartData } from '../smallBar/IBarChartData';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';

const defaultData: IBarChartData<IBarChartRowData> = {
  labels: ['Operating System', 'Allocated', 'Used', 'Unused', 'Percentage Used'],
  datasets: [
    {
      label: 'Windows',
      capacity: 12,
      used: 7.2,
      available: 4.8,
    },
    {
      label: 'Linux',
      capacity: 12,
      used: 6.4,
      available: 1.6,
    },
    {
      label: 'Solaris',
      capacity: 12,
      used: 1,
      available: 4,
    },
    {
      label: 'Neptune',
      capacity: 12,
      used: 2,
      available: 1,
    },
    {
      label: 'GalaxyOS',
      capacity: 12,
      used: 12.8,
      available: 3.2,
    },
    {
      label: 'Cosmos',
      capacity: 12,
      used: 0.5,
      available: 0.5,
    },
  ],
};

export default defaultData;
