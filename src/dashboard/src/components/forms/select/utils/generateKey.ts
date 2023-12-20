export const generateKey = (value: string | number | readonly string[] | undefined) => {
  if (value === undefined) return '';
  if (typeof value === 'string' || typeof value === 'number') return value;
  return value.join('-');
};
