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
  input: 'TB' | 'GB' | 'MB' | 'KB' | 'B' | '' = '',
  output: 'TB' | 'GB' | 'MB' | 'KB' | 'B' | '' = '',
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
    else if (output === 'B') result = value * Math.pow(1024, 4);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'GB') {
    if (output === 'TB') result = value / 1024;
    else if (output === 'MB') result = value * Math.pow(1024, 2);
    else if (output === 'KB') result = value * Math.pow(1024, 3);
    else if (output === 'B') result = value * Math.pow(1024, 4);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'MB') {
    if (output === 'TB') result = value / Math.pow(1024, 2);
    else if (output === 'GB') result = value / 1024;
    else if (output === 'KB') result = value * Math.pow(1024, 3);
    else if (output === 'B') result = value * Math.pow(1024, 4);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'KB') {
    if (output === 'TB') result = value / Math.pow(1024, 3);
    else if (output === 'GB') result = value / Math.pow(1024, 2);
    else if (output === 'MB') result = value / 1024;
    else if (output === 'B') result = value * Math.pow(1024, 4);
    else if (output === '') result = value * Math.pow(1024, 4);
  } else if (input === 'B') {
    if (output === 'TB') result = value / Math.pow(1024, 4);
    else if (output === 'GB') result = value / Math.pow(1024, 3);
    else if (output === 'MB') result = value / Math.pow(1024, 2);
    else if (output === 'KB') result = value / 1024;
  } else if (input === '') {
    if (output === 'TB') result = value / Math.pow(1024, 4);
    else if (output === 'GB') result = value / Math.pow(1024, 3);
    else if (output === 'MB') result = value / Math.pow(1024, 2);
    else if (output === 'KB') result = value / 1024;
  }
  if (result > 0 && (options?.type === 'string' || options?.type === undefined)) {
    // Downsize to the smaller type.
    while (result < 1 && output !== '') {
      const values = reduceToType(result, output);
      result = values.value;
      output = values.type;
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

/**
 *
 * @param value value to reduce if less than 1.
 * @param type The type the value is currently in.
 * @returns
 */
export const reduceToType = (
  value: number,
  type: 'TB' | 'GB' | 'MB' | 'KB' | 'B' | '' = '',
): { value: number; type: 'TB' | 'GB' | 'MB' | 'KB' | 'B' | '' } => {
  if (value >= 1) return { value, type };
  // Downsize to the smaller type.
  if (type === 'TB') {
    return {
      value: value * 1024,
      type: 'GB',
    };
  } else if (type === 'GB') {
    return {
      value: value * 1024,
      type: 'MB',
    };
  } else if (type === 'MB') {
    return {
      value: value * 1024,
      type: 'KB',
    };
  } else if (type === 'KB') {
    return {
      value: value * 1024,
      type: 'B',
    };
  }
  return { value, type };
};
