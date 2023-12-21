import styles from './Chart.module.scss';
import { Button } from '@/components/buttons';

export const AllOrgDonutChart: React.FC = () => {
  return (
    <div className={styles.panel}>
      <h1>All Organizations</h1>
      <div className={styles.info}>
        <h2>Totals for 6 organizations</h2>
        <div>
          <p>
            Allocated <span>50 TB</span>
          </p>
        </div>
        <div>
          <p>
            Used <span>50 TB</span>
          </p>
        </div>
        <div>
          <p>
            Unused <span>50 TB</span>
          </p>
        </div>
      </div>
      <div className={styles.chart}></div>
      <Button variant="secondary" iconPath="/images/download-icon.png">
        Export to Excel
      </Button>
    </div>
  );
};
