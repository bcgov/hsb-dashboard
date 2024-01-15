import styles from './Select.module.scss';

import { Spinner } from '@/components';
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
  loading?: boolean;
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
  loading,
  onChange,
  onBlur,
}: ISelectProps<T>) => {
  const [selected, setSelected] = React.useState<string | number | readonly string[] | undefined>(
    value,
  );

  const variantClassName = variant === 'filter' ? styles.dropdown : styles.simpleDropdown;
  const multiSelectClassName = multiple === true ? styles.multiSelect : '';
  const selectRef = React.useRef<HTMLSelectElement>(null);

  React.useEffect(() => {
    setSelected(value);
  }, [value]);

  React.useEffect(() => {
    if (multiple && selectRef.current) {
      selectRef.current.scrollTop = 0;
    }
  }, [multiple, options.length]);

  const handleBlur: FocusEventHandler<HTMLSelectElement> = (e) => {
    onBlur?.(e);

    if (multiple && selectRef.current) {
      selectRef.current.scrollTop = 0;
    }
  };

  return (
    <div className={`${variantClassName} ${multiSelectClassName} ${className}`}>
      {label && <label htmlFor={id}>{label}</label>}
      {loading && <Spinner className={styles.spinner} />}
      <select
        id={id}
        name={name}
        title={title}
        disabled={disabled}
        multiple={multiple}
        ref={selectRef}
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
        onBlur={handleBlur}
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
