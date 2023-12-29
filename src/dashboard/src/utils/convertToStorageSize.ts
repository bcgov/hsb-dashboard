/**
 * Converts a storage value to a specified size type.
 * @param value The initial value to convert.
 * @param input The input size type.
 * @param output The output size type.
 * @returns A string representing the storage size.
 */
export const convertToStorageSize = (
  value: number,
  input: 'TB' | 'GB' | 'MB' | 'KB' | '' = '',
  output: 'TB' | 'GB' | 'MB' | 'KB' | '' = '',
  locales: Intl.LocalesArgument = navigator.language,
  options?: ({ formula: (value: number) => number } & Intl.NumberFormatOptions) | undefined,
) => {
  var result = value;
  if (input === output) result = value;
  else if (input === 'TB') {
    if (output === 'GB') result = value * 1024;
    else if (output === 'MB') result = value * Math.pow(1024, 2);
    else if (output === 'KB') result = value * Math.pow(1024, 3);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'GB') {
    if (output === 'TB') result = value / 1024;
    else if (output === 'MB') result = value * Math.pow(1024, 2);
    else if (output === 'KB') result = value * Math.pow(1024, 3);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'MB') {
    if (output === 'TB') result = value / Math.pow(1024, 2);
    else if (output === 'GB') result = value / 1024;
    else if (output === 'KB') result = value * Math.pow(1024, 3);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'KB') {
    if (output === 'TB') result = value / Math.pow(1024, 3);
    else if (output === 'GB') result = value / Math.pow(1024, 2);
    else if (output === 'MB') result = value / 1024;
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === '') {
    if (output === 'TB') result = value / Math.pow(1024, 4);
    else if (output === 'GB') result = value / Math.pow(1024, 3);
    else if (output === 'MB') result = value / Math.pow(1024, 2);
    else if (output === 'KB') result = value / 1024;
  }
  return `${(options?.formula(result) ?? result).toLocaleString(
    locales,
    options,
  )} ${output}`.trimEnd();
};
