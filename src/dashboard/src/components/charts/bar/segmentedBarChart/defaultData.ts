export const defaultData = (numDrives: number, labelsArray: string[]) => {
  const datasets = [];
  // Define color pairs for used and unused space
  const colorPairs = [
    ['#003366', '#519CE8'],
    ['#DF9901', '#FFC342'],
    ['#A9A9A9', '#D7D7D7'],
  ];

  for (let drive = 1; drive <= numDrives; drive++) {
    // Generate random data for the example
    const usedSpaceData = labelsArray.map(() => Math.floor(Math.random() * 500 + 100));
    const totalSpaceData = labelsArray.map(() => Math.floor(Math.random() * 500 + 600));
    const unusedSpaceData = totalSpaceData.map((total, i) => total - usedSpaceData[i]);

    // Get color pair based on the current drive, looping if necessary.
    const colors = colorPairs[(drive - 1) % colorPairs.length];

    datasets.push({
      label: `Used Space Drive ${drive} (GB)`,
      data: usedSpaceData,
      backgroundColor: colors[0],
      stack: `Stack ${drive - 1}`,
    });

    datasets.push({
      label: `Unused Space Drive ${drive} (GB)`,
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
