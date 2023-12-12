import styles from './Table.module.scss';

export interface ITableHeaderProps<T> {
  data: T[];
}

export interface ITableRowProps<T> {
  data: T;
  index: number;
}

export interface ITableFooterProps<T> {
  data: T[];
}

export interface ITableProps<T> {
  data: T[];
  rowKey?: keyof T | ((data: T, index: number) => React.Key | null | undefined);
  showHeader?: boolean;
  header?: React.ReactNode | ((props: ITableHeaderProps<T>) => React.ReactNode);
  children?: React.ReactNode | ((props: ITableRowProps<T>) => React.ReactNode);
  showFooter?: boolean;
  footer?: React.ReactNode | ((props: ITableFooterProps<T>) => React.ReactNode);
}

export const Table = <T extends unknown>({
  data,
  showHeader = true,
  header,
  rowKey = (_, index) => index,
  children,
  showFooter,
  footer,
}: ITableProps<T>) => {
  return (
    <div className={styles.table}>
      {showHeader && (
        <div className={styles.tableHeader}>
          {typeof header === 'function'
            ? (header as (props: ITableHeaderProps<T>) => React.ReactNode)({ data })
            : header}
        </div>
      )}
      <div className={styles.tableRows}>
        {data.map((row, index) => {
          return (
            <div
              key={typeof rowKey === 'function' ? rowKey(row, index) : `${row[rowKey]}`}
              className={styles.tableRow}
            >
              {typeof children === 'function'
                ? (children as (props: ITableRowProps<T>) => React.ReactNode)({ data: row, index })
                : children}
            </div>
          );
        })}
      </div>
      {showFooter && (
        <div className={styles.tableFooter}>
          {typeof footer === 'function'
            ? (footer as (props: ITableFooterProps<T>) => React.ReactNode)({ data })
            : footer}
        </div>
      )}
    </div>
  );
};
