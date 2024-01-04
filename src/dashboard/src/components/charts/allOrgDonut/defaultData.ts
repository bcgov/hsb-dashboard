// Data for the Donut Chart with specified colors
export const defaultData = {
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
