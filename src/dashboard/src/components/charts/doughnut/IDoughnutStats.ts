import { ChartData } from 'chart.js';

export interface IDoughnutStats {
  space: string;
  used: string;
  available: string;
  usedPercent: number;
  availablePercent: number;
  chart: ChartData<'doughnut', number[], string>;
}
