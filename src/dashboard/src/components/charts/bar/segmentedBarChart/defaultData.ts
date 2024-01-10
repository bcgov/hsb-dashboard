export const defaultData = () => {
    const usedSpaceData1 = [320, 400, 450, 480, 500, 520, 540, 560, 580, 600, 620, 650];
    const totalSpaceData1 = [1000, 900, 800, 1200, 500, 700, 600, 800, 800, 700, 700, 850];
    const unusedSpaceData1 = totalSpaceData1.map((total, i) => total - usedSpaceData1[i]);
  
    const usedSpaceData3 = [220, 280, 300, 350, 370, 390, 410, 430, 440, 460, 490, 510];
    const totalSpaceData3 = [700, 700, 700, 700, 700, 700, 700, 700, 700, 700, 700, 700];
    const unusedSpaceData3 = totalSpaceData3.map((total, i) => total - usedSpaceData3[i]);
  
    const usedSpaceData4 = [120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230];
    const totalSpaceData4 = [300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300, 300];
    const unusedSpaceData4 = totalSpaceData4.map((total, i) => total - usedSpaceData4[i]);
  
    return {
    labels: [
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
    ],
    datasets: [
      {
        label: 'Used Space Drive 1 (GB)',
        data: usedSpaceData1,
        backgroundColor: '#003366',
        stack: 'Stack 0',
      },
      {
        label: 'Unused Space Drive 1 (GB)',
        data: unusedSpaceData1,
        backgroundColor: '#519CE8',
        stack: 'Stack 0',
      },
      {
        label: 'Used Space Drive 2 (GB)',
        data: usedSpaceData3,
        backgroundColor: '#DF9901',
        stack: 'Stack 1',
      },
      {
        label: 'Unused Space Drive 2 (GB)',
        data: unusedSpaceData3,
        backgroundColor: '#FFC342',
        stack: 'Stack 1',
      },
      {
        label: 'Used Space Drive 3 (GB)',
        data: usedSpaceData4,
        backgroundColor: '#A9A9A9',
        stack: 'Stack 2',
      },
      {
        label: 'Unused Space Drive 3 (GB)',
        data: unusedSpaceData4,
        backgroundColor: '#D7D7D7',
        stack: 'Stack 2',
      },
    ],
  };
};
