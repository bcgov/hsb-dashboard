'use client';

import React from 'react';
import styles from './Chart.module.scss';
import { Button } from '@/components/buttons';
import { Doughnut } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip } from 'chart.js';

ChartJS.register(ArcElement, Tooltip);

// Data for the Donut Chart with specified colors
const data = {
    labels: ['Unused', 'Used', 'Allocated'], // Labels to match ring order
    datasets: [
      {
        label: 'Unused', // Outer ring
        data: [25, 0, 0], // Represents 25% of the ring 
        backgroundColor: ['#C4C7CA'],
        borderColor: ['#C4C7CA'],
        circumference: 90, // Quarter of the circle
      },
      {
        label: 'Used', // Middle ring
        data: [75, 0, 0], // Represents 75% of the ring
        backgroundColor: ['#003366'],
        borderColor: ['#003366'],
        circumference: 270, // Three quarters of the circle
      },
      {
        label: 'Allocated', // Inner ring
        data: [100, 0, 0], // Represents full 100% of the ring
        backgroundColor: ['#DF9901'],
        borderColor: ['#DF9901'],
      },
    ],
};

export const AllOrgDonutChart: React.FC = () => {
  return (
    <div className={styles.panel}>
      <h1>All Organizations</h1>
      <div className={styles.chartContainer}>
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
        <div className={styles.chart}>
        <Doughnut
          data={data}
          options={{
            rotation: 180, // Start the chart at the top
            cutout: '75%', // Adjust the cutout size to show the rings as desired
            plugins: {
                legend: {
                  display: false // Hide the legend
                },
                tooltip: {
                  callbacks: {
                    title: function() {
                      return ''; // Return an empty string to remove the title for tooltips
                    },
                  }
                }
              },
            // Ensure that each segment starts from the same point
            circumference: 360, // Full circle for all rings
          }}
        />
        <p className={styles.total}>50 TB <span>Total</span></p>
        </div>
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png">
        Export to Excel
      </Button>
    </div>
  );
};
