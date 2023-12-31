import { ChartData } from 'chart.js';

export const defaultData: ChartData<'line', number[], string> = {
  labels: Array.from(new Array(12), (val, index) => `Month ${index + 1}`),
  datasets: [
    {
      label: 'Total Used in TB',
      data: Array.from(new Array(12), (_, i) => Math.random() * 10 + 50), // Randomly generated data
      borderColor: '#313132',
      backgroundColor: '#313132',
      fill: false,
    },
    {
      label: 'Total Allocated in TB',
      data: Array.from(new Array(12), (_, i) => Math.random() * 10 + 70), // Randomly generated data
      borderColor: '#476E94',
      backgroundColor: '#476E94',
      fill: false,
    },
  ],
};
