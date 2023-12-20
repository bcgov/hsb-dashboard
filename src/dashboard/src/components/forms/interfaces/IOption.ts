export interface IOption<T extends unknown> {
  label: React.ReactNode;
  value: string | number | readonly string[] | undefined;
  data: T;
}
