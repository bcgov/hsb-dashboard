/**
 * Calculate the percentage of used capacity
 * @param capacity - The total capacity
 * @param available - The available capacity
 * @returns The percentage of capacity used, rounded to the nearest whole number
 */
export const calcPercentageUsed = (available: number, capacity: number) => {
  return capacity ? Math.round(((capacity - available) / capacity) * 100) : 0;
};

/**
 * Calculate the percentage of unused capacity
 * @param available - The available capacity
 * @param capacity - The total capacity
 * @returns The percentage of capacity unused, rounded to the nearest whole number
 */
export const calcPercentageUnused = (available: number, capacity: number) => {
  return 100 - calcPercentageUsed(available, capacity);
};
