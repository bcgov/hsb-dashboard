import { IDoughnutStats } from '@/components/charts';

export const defaultData: IDoughnutStats = {
  space: '0 GB',
  used: '0 GB',
  usedPercent: 0,
  available: '0 GB',
  availablePercent: 0,
  chart: {
    labels: ['Used', 'Unused'],
    datasets: [
      {
        data: [0, 0], // Data for 'Used' and 'Unused'
        backgroundColor: ['#DF9901', '#FFECC2'], // Colors for 'Used' and 'Unused'
        borderColor: ['#DF9901', '#FFECC2'], // Border colors for 'Used' and 'Unused'
        borderWidth: 1,
      },
    ],
  },
};
