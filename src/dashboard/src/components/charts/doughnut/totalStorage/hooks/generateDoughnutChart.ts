import { convertToStorageSize } from '@/utils';
import { IDoughnutStats } from '../..';

export const generateDoughnutChart = (
  space: number | undefined = 0,
  available: number | undefined = 0,
  data: IDoughnutStats,
) => {
  const used = space - available;
  const usedPercent = space ? (used / space) * 100 : 0;
  const availablePercent = space ? (available / space) * 100 : 0;

  return {
    space: convertToStorageSize<string>(space, 'MB', 'TB', {
      formula: Math.trunc,
    }),
    used: convertToStorageSize<string>(used, 'MB', 'TB', {
      formula: Math.trunc,
    }),
    available: convertToStorageSize<string>(available, 'MB', 'TB', {
      formula: Math.trunc,
    }),
    usedPercent,
    availablePercent,
    chart: {
      ...data.chart,
      datasets: [
        {
          ...data.chart.datasets[0],
          data: [used, available],
        },
      ],
    },
  };
};
