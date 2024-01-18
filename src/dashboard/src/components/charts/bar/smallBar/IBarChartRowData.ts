export interface IBarChartRowData<T> {
  key: string;
  label: string;
  capacity: number;
  available: number;
  data?: T;
}
