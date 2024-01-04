import styles from './Select.module.scss';

import { uniqueId } from 'lodash';
import React, { FocusEventHandler } from 'react';
import { FormError, IOption } from '..';
import { generateKey } from './utils';

export interface ISelectProps<T> {
  variant?: 'primary' | 'filter';
  options: IOption<T>[];
  label?: string;
  title?: string;
  id?: string;
  name?: string;
  value?: string | number | readonly string[];
  className?: string;
  disabled?: boolean;
  placeholder?: string;
  error?: React.ReactNode;
  multiple?: boolean;
  required?: boolean;
  onChange?: (
    values: string | number | readonly string[] | undefined,
    event?: React.FormEvent<HTMLSelectElement>,
  ) => void;
  onBlur?: FocusEventHandler<HTMLSelectElement>;
}

export const Select = <T extends unknown>({
  variant = 'primary',
  options,
  label,
  title,
  id = uniqueId(),
  name,
  value,
  className = '',
  disabled,
  placeholder,
  required,
  error,
  multiple,
  onChange,
  onBlur,
}: ISelectProps<T>) => {
  const [selected, setSelected] = React.useState<string | number | readonly string[] | undefined>(
    value,
  );

  const selectClasses =
    variant === 'filter' ? `${styles.dropdown} ${className}` : styles.simpleDropdown;

  React.useEffect(() => {
    setSelected(value);
  }, [value]);

  return (
    <div className={selectClasses}>
      {label && <label htmlFor={id}>{label}</label>}
      <select
        id={id}
        name={name}
        title={title}
        disabled={disabled}
        multiple={multiple}
        value={
          !multiple
            ? options.find((o) => o.value == selected)?.value ?? ''
            : options.filter((o) => o.value == selected).map((o) => `${o.value}`)
        }
        onChange={(e) => {
          if (!multiple) {
            if (!onChange) setSelected(e.target.value);
            onChange?.(e.target.value, e);
          } else if (Array.isArray(selected)) {
            const values =
              selected.indexOf(e.target.value) === -1
                ? [...selected, e.target.value]
                : selected.filter((s) => s != e.target.value);
            if (!onChange) setSelected(values);
            onChange?.(values, e);
          }
        }}
        onBlur={onBlur}
      >
        {placeholder && (
          <option className="placeholder" value="" disabled={required}>
            {placeholder}
          </option>
        )}
        {options.map((option) => {
          return (
            <option
              key={generateKey(option.value)}
              value={option.value}
              className={
                (Array.isArray(selected) && selected.some((o) => o == option.value)) ||
                selected === option.value
                  ? styles.selected
                  : ''
              }
              onClick={(e) => {
                if (Array.isArray(selected) && selected.some((o) => o == option.value)) {
                  // Remove selected value.
                  const value = `${option.value}`;
                  const values =
                    selected.indexOf(value) === -1
                      ? [...selected, value]
                      : selected.filter((s) => s != value);
                  if (!onChange) setSelected(values);
                  onChange?.(values);
                }
              }}
            >
              {option.label}
            </option>
          );
        })}
      </select>
      <FormError message={error} />
    </div>
  );
};
