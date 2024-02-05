import { LoadingAnimation } from '..';
import { ITableFooterProps } from './ITableFooterProps';
import { ITableHeaderProps } from './ITableHeaderProps';
import { ITableRowProps } from './ITableRowProps';
import styles from './Table.module.scss';

export interface ITableProps<T extends object> {
  data?: T[];
  rows?: ITableRowProps<T>[];
  rowKey?: keyof T | ((data: T, index: number) => React.Key | null | undefined);
  showHeader?: boolean;
  header?: React.ReactNode | ((props: ITableHeaderProps<T>) => React.ReactNode);
  children?: React.ReactNode | ((props: ITableRowProps<T>) => React.ReactNode);
  showFooter?: boolean;
  footer?: React.ReactNode | ((props: ITableFooterProps<T>) => React.ReactNode);
}

export const Table = <T extends object>({
  data,
  rows: initRows,
  showHeader = true,
  header,
  rowKey = (_, index) => index,
  children,
  showFooter,
  footer,
}: ITableProps<T>) => {
  if (!data && !initRows) throw Error("Component requires either 'data' or 'rows'.");

  const rows: ITableRowProps<T>[] =
    initRows ?? data!.map((data, index) => ({ data: data, index, loading: false }));

  return (
    <div className={styles.table}>
      {showHeader && (
        <div className={styles.tableHeader}>
          {typeof header === 'function'
            ? (header as (props: ITableHeaderProps<T>) => React.ReactNode)({
                data: rows.map((r) => r.data),
              })
            : header}
        </div>
      )}
      <div className={styles.tableRows}>
        {rows.map((row, index) => {
          return (
            <div
              key={typeof rowKey === 'function' ? rowKey(row.data, index) : `${row.data[rowKey]}`}
              className={styles.tableRow}
            >
              {row.loading && <LoadingAnimation className={styles.loadingAnimation} />}
              {typeof children === 'function'
                ? (children as (props: ITableRowProps<T>) => React.ReactNode)(row)
                : children}
            </div>
          );
        })}
      </div>
      {showFooter && (
        <div className={styles.tableFooter}>
          {typeof footer === 'function'
            ? (footer as (props: ITableFooterProps<T>) => React.ReactNode)({
                data: rows.map((r) => r.data),
              })
            : footer}
        </div>
      )}
    </div>
  );
};
