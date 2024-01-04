/**
 * Converts a storage value to a specified size type.
 * @param value The initial value to convert.
 * @param input The input size type.
 * @param output The output size type.
 * @param options Configuration options.
 * @returns A string representing the storage size.
 */
export const convertToStorageSize = <T extends string | number>(
  value: number,
  input: 'TB' | 'GB' | 'MB' | 'KB' | '' = '',
  output: 'TB' | 'GB' | 'MB' | 'KB' | '' = '',
  options?:
    | ({
        formula?: (value: number) => number;
        type?: 'string' | 'number';
        locales?: Intl.LocalesArgument;
      } & Intl.NumberFormatOptions)
    | undefined,
): T => {
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
  if (result < 1) {
    // Downsize to the smaller type.
    if (output === 'TB') {
      output = 'GB';
      result *= 1024;
    } else if (output === 'GB') {
      output = 'MB';
      result *= 1024;
    } else if (output === 'MB') {
      output = 'KB';
      result *= 1024;
    } else if (output === 'KB') {
      output = '';
      result *= 1024;
    }
  }
  result = options?.formula?.(result) ?? result;

  return (
    options?.type === 'number'
      ? result
      : `${result.toLocaleString(
          options?.locales ?? navigator.language,
          options,
        )} ${output}`.trimEnd()
  ) as T;
};
