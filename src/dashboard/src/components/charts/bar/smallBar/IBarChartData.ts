import { IBarChartRowData } from './IBarChartRowData';

export interface IBarChartData<T extends IBarChartRowData<unknown>> {
  labels: string[];
  datasets: T[];
}
