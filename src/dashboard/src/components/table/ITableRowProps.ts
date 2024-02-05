export interface ITableRowProps<T extends object> {
  data: T;
  index: number;
  loading?: boolean;
}
