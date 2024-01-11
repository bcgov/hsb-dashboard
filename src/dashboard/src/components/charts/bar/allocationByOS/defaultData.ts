import { IBarChartData } from '../smallBar/IBarChartData';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';

const defaultData: IBarChartData<IBarChartRowData> = {
  labels: ['Operating System', 'Allocated', 'Used', 'Unused', 'Percentage Used'],
  datasets: [
    {
      key: 'windows',
      label: 'Windows',
      capacity: 12,
      available: 4.8,
    },
    {
      key: 'linux',
      label: 'Linux',
      capacity: 12,
      available: 1.6,
    },
    {
      key: 'solaris',
      label: 'Solaris',
      capacity: 12,
      available: 4,
    },
    {
      key: 'neptune',
      label: 'Neptune',
      capacity: 12,
      available: 1,
    },
    {
      key: 'galaxyOS',
      label: 'GalaxyOS',
      capacity: 12,
      available: 3.2,
    },
    {
      key: 'cosmos',
      label: 'Cosmos',
      capacity: 12,
      available: 0.5,
    },
  ],
};

export default defaultData;
