'use client';

import styles from './AllOrganizations.module.scss';

import { Button } from '@/components/buttons';
import { useServerItemsDoughnutChart } from '@/components/charts/hooks/useServerItemsDoughnutChart';
import { useDashboard } from '@/store';
import { ArcElement, Chart as ChartJS, Tooltip } from 'chart.js';
import React from 'react';
import { Doughnut } from 'react-chartjs-2';

ChartJS.register(ArcElement, Tooltip);

export const AllOrganizations: React.FC = () => {
  const organizations = useDashboard((state) => state.organizations);
  const data = useServerItemsDoughnutChart();

  return (
    <div className={styles.panel}>
      <h1>All Organizations</h1>
      <div className={styles.chartContainer}>
        <div className={styles.info}>
          <h2>Totals for {organizations.length} organizations</h2>
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
      <Button variant="secondary" iconPath="/images/download-icon.png" disabled>
        Export to Excel
      </Button>
    </div>
  );
};
