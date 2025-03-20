export const calcColorByUsage = (usage: number): string => {
  if (usage < 50) {
    return '#42814A';
  } else if (usage < 80) {
    return '#F8BB47';
  } else {
    return '#CE3E39';
  }
};
