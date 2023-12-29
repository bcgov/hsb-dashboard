'use client';

import { Button } from '@/components/buttons';
import { useFiltered } from '@/store';
import { convertToStorageSize } from '@/utils';
import { ArcElement, Chart as ChartJS, Tooltip } from 'chart.js';
import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import styles from './AllOrgDonutChart.module.scss';
import { IStats } from './IStats';

ChartJS.register(ArcElement, Tooltip);

// Data for the Donut Chart with specified colors

const defaultData = {
  space: '0 TB',
  used: '0 TB',
  available: '0 TB',
  chart: {
    labels: ['Unused', 'Used', 'Allocated'], // Labels to match ring order
    datasets: [
      {
        label: 'Unused', // Outer ring
        data: [0, 0, 0], // Represents 25% of the ring
        backgroundColor: ['#C4C7CA'],
        borderColor: ['#C4C7CA'],
        circumference: 360, // Quarter of the circle
      },
      {
        label: 'Used', // Middle ring
        data: [0, 0, 0], // Represents 75% of the ring
        backgroundColor: ['#003366'],
        borderColor: ['#003366'],
        circumference: 360, // Three quarters of the circle
      },
      {
        label: 'Allocated', // Inner ring
        data: [0, 0, 0], // Represents full 100% of the ring
        backgroundColor: ['#DF9901'],
        borderColor: ['#DF9901'],
      },
    ],
  },
};

export const AllOrgDonutChart: React.FC = () => {
  const organization = useFiltered((state) => state.organization);
  const organizations = useFiltered((state) => state.organizations);
  const servers = useFiltered((state) => state.serverItems);

  const [data, setData] = React.useState<IStats>(defaultData);

  React.useEffect(() => {
    if (servers.length) {
      const space = servers.map((si) => si.capacity!).reduce((a, b) => (a ?? 0) + (b ?? 0));
      const available = servers
        .map((si) => si.availableSpace!)
        .reduce((a, b) => (a ?? 0) + (b ?? 0));
      const used = space - available;
      const usedPer = (used / space) * 100;
      const availablePer = (available / space) * 100;
      setData((data) => ({
        space: convertToStorageSize(space, 'MB', 'TB', navigator.language, { formula: Math.trunc }),
        used: convertToStorageSize(used, 'MB', 'TB', navigator.language, {
          formula: Math.trunc,
        }),
        available: convertToStorageSize(available, 'MB', 'TB', navigator.language, {
          formula: Math.trunc,
        }),
        chart: {
          ...data.chart,
          datasets: [
            {
              ...data.chart.datasets[0],
              data: [usedPer, 0, 0],
              circumference: (360 * usedPer) / 100,
            },
            {
              ...data.chart.datasets[1],
              data: [availablePer, 0, 0],
              circumference: (360 * availablePer) / 100,
            },
            { ...data.chart.datasets[2], data: [100, 0, 0] },
          ],
        },
      }));
    } else {
      setData(defaultData);
    }
  }, [servers]);

  return (
    <div className={styles.panel}>
      <h1>All Organizations</h1>
      <div className={styles.chartContainer}>
        <div className={styles.info}>
          <h2>Totals for {organization ? 1 : organizations.length} organizations</h2>
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
              cutout: '70%', // Adjust the cutout size to show the rings as desired
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
