export interface ITableRowData<T> {
  server: string;
  tenant: string;
  os: string;
  capacity: number;
  available: number;
  data?: T;
}
