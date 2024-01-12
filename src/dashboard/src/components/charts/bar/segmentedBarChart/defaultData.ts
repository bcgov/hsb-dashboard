import { ChartData } from 'chart.js';

// Define color pairs for used and unused space
const colorPairs = [
  ['#4D7194', '#86BAEF'],
  ['#E9B84E', '#FFD57B'],
  ['#A9A9A9', '#D7D7D7'],
];

const labelsArray = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];

export const defaultData = (numDrives: number): ChartData<'bar', number[], string> => {
  const datasets = [];

  for (let drive = 1; drive <= numDrives; drive++) {
    // Generate random data for the example
    const usedSpaceData = labelsArray.map(() => Math.floor(Math.random() * 500 + 100));
    const totalSpaceData = labelsArray.map(() => Math.floor(Math.random() * 500 + 600));
    const unusedSpaceData = totalSpaceData.map((total, i) => total - usedSpaceData[i]);

    // Get color pair based on the current drive, looping if necessary.
    const colors = colorPairs[(drive - 1) % colorPairs.length];

    datasets.push({
      label: `Used Drive ${drive} (GB)`,
      data: usedSpaceData,
      backgroundColor: colors[0],
      stack: `Stack ${drive - 1}`,
    });

    datasets.push({
      label: `Unused Drive ${drive} (GB)`,
      data: unusedSpaceData,
      backgroundColor: colors[1],
      stack: `Stack ${drive - 1}`,
    });
  }

  return {
    labels: labelsArray,
    datasets: datasets,
  };
};
