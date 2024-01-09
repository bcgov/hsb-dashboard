import { Button } from '@/components';
import { BarRow } from './BarRow';
import { IBarChartData } from './IBarChartData';
import { IBarChartRowData } from './IBarChartRowData';
import styles from './SmallBarChart.module.scss';

export interface ISmallBarChartProps<T extends IBarChartRowData> {
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
}

export const SmallBarChart = <T extends IBarChartRowData>({
  title,
  data,
  children,
  exportDisabled,
  onExport,
}: ISmallBarChartProps<T>) => {
  return (
    <div className={styles.panel}>
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
                    used={row.used}
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
