// Configuration for the line chart
export const defaultChartOptions = {
  responsive: true,
  maintainAspectRatio: true,
  plugins: {
    legend: {
      position: 'bottom' as const,
      align: 'start' as const,
      labels: {
        boxWidth: 18,
        padding: 20,
        color: '#313132',
        font: {
          size: 16,
          fontFamily: 'BCSans',
        },
      },
    },
    title: {
      display: false,
    },
  },
  layout: {
    padding: {
      top: 20,
    },
  },
};
