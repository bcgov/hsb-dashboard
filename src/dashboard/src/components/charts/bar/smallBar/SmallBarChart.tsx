import { Button } from '@/components';
import { LoadingAnimation } from '../../../loadingAnimation';
import { BarRow } from './BarRow';
import { IBarChartData } from './IBarChartData';
import { IBarChartRowData } from './IBarChartRowData';
import styles from './SmallBarChart.module.scss';

export interface ISmallBarChartProps<T extends IBarChartRowData<unknown>> {
  /** Title header of the component */
  title?: string;
  /** Data to be displayed in the bar chart */
  data: IBarChartData<T>;
  /** Whether the export to Excel is disabled */
  exportDisabled?: boolean;
  /** Event fires when the export to Excel is clicked */
  onExport?: () => void;
  /** Children bar char row output */
  children?: React.ReactNode | ((data: IBarChartData<T>) => React.ReactNode);
  loading?: boolean;
}

export const SmallBarChart = <T extends IBarChartRowData<unknown>>({
  title,
  data,
  children,
  exportDisabled,
  onExport,
  loading,
}: ISmallBarChartProps<T>) => {
  return (
    <div className={styles.panel}>
      {loading && <LoadingAnimation />}
      {title && <h1>{title}</h1>}
      <div className={styles.chartContainer}>
        <div className={styles.headings}>
          {data.labels.map((label) => (
            <p key={label}>{label}</p>
          ))}
        </div>
        <div className={styles.chart}>
          {typeof children === 'function'
            ? children(data)
            : children
            ? children
            : data.datasets.map((row) => {
                return (
                  <BarRow
                    key={row.label}
                    label={row.label}
                    capacity={row.capacity}
                    available={row.available}
                  />
                );
              })}
        </div>
      </div>
      {onExport && (
        <Button
          variant="secondary"
          iconPath="/images/download-icon.png"
          disabled={exportDisabled}
          onClick={() => onExport?.()}
        >
          Export to Excel
        </Button>
      )}
    </div>
  );
};
