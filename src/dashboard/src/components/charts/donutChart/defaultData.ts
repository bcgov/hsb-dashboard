const totalSpaceGB = 100;
const usedSpaceGB = 75;
const availableSpaceGB = totalSpaceGB - usedSpaceGB;
const usedPercentage = (usedSpaceGB / totalSpaceGB) * 100

export const defaultData = {
  space: totalSpaceGB,
  used: usedSpaceGB,
  available: availableSpaceGB,

  labels: ['Used', 'Unused'],
  datasets: [
    {
      data: [usedSpaceGB, availableSpaceGB], // Data for 'Used' and 'Unused'
      backgroundColor: ['#DF9901', '#FFECC2'], // Colors for 'Used' and 'Unused'
      borderColor: ['#DF9901', '#FFECC2'], // Border colors for 'Used' and 'Unused'
      borderWidth: 1,
    }
  ]
};