import { ChartData } from 'chart.js';

export interface IStats {
  space: string;
  used: string;
  available: string;
  chart: ChartData<'doughnut', number[], string>;
}
