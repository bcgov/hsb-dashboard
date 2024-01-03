// Data for the Donut Chart with specified colors
export const defaultData = {
  space: '10 TB',
  used: '20 TB',
  available: '30 TB',
  chart: {
    labels: ['Unused', 'Used', 'Allocated'], // Labels to match ring order
    datasets: [
      {
        label: 'Unused', // Outer ring
        data: [10, 0, 0], // Represents 25% of the ring
        backgroundColor: ['#C4C7CA'],
        borderColor: ['#C4C7CA'],
        circumference: 120, // Quarter of the circle
      },
      {
        label: 'Used', // Middle ring
        data: [20, 0, 0], // Represents 75% of the ring
        backgroundColor: ['#003366'],
        borderColor: ['#003366'],
        circumference: 260, // Three quarters of the circle
      },
      {
        label: 'Allocated', // Inner ring
        data: [30, 0, 0], // Represents full 100% of the ring
        backgroundColor: ['#DF9901'],
        borderColor: ['#DF9901'],
      },
    ],
  },
};
