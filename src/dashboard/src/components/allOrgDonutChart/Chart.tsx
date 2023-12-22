'use client';

import React from 'react';
import styles from './Chart.module.scss';
import { Button } from '@/components/buttons';
import { Doughnut } from 'react-chartjs-2';
import { Chart as ChartJS, ArcElement, Tooltip } from 'chart.js';

ChartJS.register(ArcElement, Tooltip);

// Updated Data for the Donut Chart with specified colors
const data = {
    labels: ['Unused', 'Used', 'Allocated'], // Updated labels to match ring order
    datasets: [
      {
        label: 'Unused', // Outer ring
        data: [25, 0, 0], // Represents 25% of the ring 
        backgroundColor: ['#D9D9D9'],
        borderColor: ['#D9D9D9'],
        // Add css style for a circular cutout in the middle
        circumference: 60, // Full circle
      },
      {
        label: 'Used', // Middle ring
        data: [75, 0, 0], // Represents 75% of the ring
        backgroundColor: ['#003366'],
        borderColor: ['#003366'],
        // Adjust the circumference to represent 75%
        circumference: 200, // Three quarters of the circle
      },
      {
        label: 'Allocated', // Inner ring
        data: [100, 0, 0], // Represents full 100% of the ring
        backgroundColor: ['#FCBA19'],
        borderColor: ['#FCBA19'],
        borderAlign: 'inner',
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
            cutout: '70%', // Adjust the cutout size to show the rings as desired
            plugins: {
                legend: {
                  display: false // Hide the legend
                },
                tooltip: {
                  callbacks: {
                    title: function() {
                      return ''; // Return an empty string to remove the title
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
