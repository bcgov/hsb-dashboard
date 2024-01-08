import { ChartData } from 'chart.js';

export interface IDoughnutStats {
  space: string;
  used: string;
  available: string;
  chart: ChartData<'doughnut', number[], string>;
}
