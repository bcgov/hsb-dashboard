import styles from './smallBarChart.module.scss';
import { BarRow } from './barRow';
import { Button } from '@/components/buttons';
import defaultData from './defaultData';

interface SmallBarChartProps {
  title: string;
}

export const SmallBarChart: React.FC<SmallBarChartProps> = ({ title }) => {
  return (
    <div className={styles.panel}>
      <h1>{title}</h1>
      <div className={styles.chartContainer}>
        <div className={styles.headings}>
          <p>{defaultData.some(item => item.drive) ? 'Drive' : 'Operating System'}</p>
          <p>Allocated</p>
          <p>Used</p>
          <p>Unused</p>
          <p>Percentage Used</p>
        </div>
        <div className={styles.chart}>
        {defaultData.map((data, index) => (
            <BarRow
              key={index}
              operatingSystem={data.operatingSystem}
              drive={data.drive}
              used={data.used}
              unused={data.unused}
            />
          ))}
        </div>
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png">
        Export to Excel
      </Button>
    </div>
  );
};
