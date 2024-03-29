'use client';

import styles from './AllOrganizations.module.scss';

import { Button } from '@/components/buttons';
import { IOrganizationListModel, IServerItemListModel } from '@/hooks';
import { ArcElement, Chart as ChartJS, Tooltip } from 'chart.js';
import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import { LoadingAnimation } from '../../../loadingAnimation';
import { generateAllOrganizationsDoughnutChart } from './utils';

ChartJS.register(ArcElement, Tooltip);

export interface IAllOrganizationsProps {
  organizations: IOrganizationListModel[];
  serverItems: IServerItemListModel[];
  loading?: boolean;
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: () => void;
}

export const AllOrganizations = ({
  organizations,
  serverItems,
  loading,
  showExport,
  exportDisabled,
  onExport,
}: IAllOrganizationsProps) => {
  const [data, setData] = React.useState(generateAllOrganizationsDoughnutChart(serverItems));

  React.useEffect(() => {
    setData(generateAllOrganizationsDoughnutChart(serverItems));
  }, [serverItems]);

  return (
    <div className={styles.panel}>
      {loading && <LoadingAnimation />}
      <h1>All Storage</h1>
      <div className={styles.chartContainer}>
        <div className={styles.info}>
          <h2>Totals for {organizations.length.toLocaleString()} organizations</h2>
          <div>
            <p>
              Allocated <span>{data.space}</span>
            </p>
          </div>
          <div>
            <p>
              Used <span>{data.used}</span>
            </p>
          </div>
          <div>
            <p>
              Unused <span>{data.available}</span>
            </p>
          </div>
        </div>
        <div className={styles.chart}>
          <Doughnut
            data={data.chart}
            options={{
              rotation: 180, // Start the chart at the top
              cutout: '75%', // Adjust the cutout size to show the rings as desired
              plugins: {
                legend: {
                  display: false, // Hide the legend
                },
                tooltip: {
                  callbacks: {
                    title: function () {
                      return ''; // Return an empty string to remove the title for tooltips
                    },
                  },
                },
              },
              // Ensure that each segment starts from the same point
              circumference: 360, // Full circle for all rings
            }}
          />
          <p className={styles.total}>
            {data.space} <span>Total</span>
          </p>
        </div>
      </div>
      {showExport && (
        <Button
          variant="secondary"
          iconPath="/images/download-icon.png"
          onClick={onExport}
          disabled={exportDisabled}
        >
          Export to Excel
        </Button>
      )}
    </div>
  );
};
