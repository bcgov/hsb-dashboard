import { convertToStorageSize } from '@/utils';
import { IStats } from './IStats';

export const updateData = (
  space: number | undefined = 0,
  available: number | undefined = 0,
  data: IStats,
) => {
  const used = space - available;
  const usedPercent = space ? (used / space) * 100 : 0;
  const availablePercent = space ? (available / space) * 100 : 0;
  const usedCir = usedPercent ? (360 * usedPercent) / 100 : 0;
  const availableCir = availablePercent ? (360 * availablePercent) / 100 : 0;

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
    chart: {
      ...data.chart,
      datasets: [
        {
          ...data.chart.datasets[0],
          data: [availablePercent, 0, 0],
          circumference: availableCir,
        },
        {
          ...data.chart.datasets[1],
          data: [usedPercent, 0, 0],
          circumference: usedCir,
        },
        { ...data.chart.datasets[2], data: [100, 0, 0] },
      ],
    },
  };
};
