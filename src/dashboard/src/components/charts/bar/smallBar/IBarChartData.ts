import { IBarChartRowData } from './IBarChartRowData';

export interface IBarChartData<T extends IBarChartRowData> {
  labels: string[];
  datasets: T[];
}
