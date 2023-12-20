import styles from './Select.module.scss';

import { uniqueId } from 'lodash';
import React, { ChangeEventHandler, FocusEventHandler } from 'react';
import { FormError, IOption } from '..';
import { generateKey } from './utils';

export interface ISelectProps<T> {
  options: IOption<T>[];
  label?: string;
  title?: string;
  id?: string;
  name?: string;
  value?: string | number | readonly string[];
  defaultValue?: string | number | readonly string[];
  className?: string;
  disabled?: boolean;
  placeholder?: string;
  error?: React.ReactNode;
  isMulti?: boolean;
  onChange?: ChangeEventHandler<HTMLSelectElement>;
  onBlur?: FocusEventHandler<HTMLSelectElement>;
}

export const Select = <T extends unknown>({
  options,
  label,
  title,
  id = uniqueId(),
  name,
  value,
  defaultValue,
  className = '',
  disabled,
  placeholder,
  error,
  isMulti,
  onChange,
  onBlur,
}: ISelectProps<T>) => {
  const initValue = value ?? defaultValue ?? (placeholder ? '' : undefined);

  const [selected, setSelected] = React.useState<string | number | readonly string[] | undefined>(
    value,
  );

  React.useEffect(() => {
    setSelected(value);
  }, [value]);

  return (
    <div className={`${styles.dropdown} ${className}`}>
      {label && <label htmlFor={id}>{label}</label>}
      <select
        id={id}
        name={name}
        title={title}
        disabled={disabled}
        value={options.find((o) => o.value == selected)?.value ?? ''}
        onChange={(e) => {
          setSelected(e.target.value);
          onChange?.(e);
        }}
        onBlur={onBlur}
      >
        {placeholder && (
          <option className="placeholder" value="" disabled>
            {placeholder}
          </option>
        )}
        {options.map((option) => {
          return (
            <option key={generateKey(option.value)} value={option.value}>
              {option.label}
            </option>
          );
        })}
      </select>
      <FormError message={error} />
    </div>
  );
};
