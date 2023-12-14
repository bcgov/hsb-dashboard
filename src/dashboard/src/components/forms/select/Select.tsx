import styles from './Select.module.scss';

import { uniqueId } from 'lodash';
import { ChangeEventHandler, FocusEventHandler } from 'react';

export interface ISelectProps<T> {
  options: T[];
  label?: string;
  title?: string;
  id?: string;
  name?: string;
  value?: string | number | readonly string[];
  defaultValue?: string | number | readonly string[];
  className?: string;
  disabled?: boolean;
  placeholder?: string;
  onChange?: ChangeEventHandler<HTMLSelectElement>;
  onBlur?: FocusEventHandler<HTMLSelectElement>;
}

export const Select = <T extends unknown>({
  label,
  title,
  id = uniqueId(),
  name,
  value,
  defaultValue,
  className = '',
  disabled,
  placeholder,
  onChange,
  onBlur,
}: ISelectProps<T>) => {
  const initValue = value ?? defaultValue ?? (placeholder ? '' : undefined);
  return (
    <div className={`${styles.container} ${className}`}>
      {label && <label htmlFor={id}>{label}</label>}
      <select
        id={id}
        name={name}
        title={title}
        className={styles.select}
        disabled={disabled}
        value={value}
        defaultValue={initValue}
        onChange={onChange}
        onBlur={onBlur}
      >
        {placeholder && (
          <option className="placeholder" value="" disabled>
            {placeholder}
          </option>
        )}
        <option>Option</option>
        <option>Option</option>
      </select>
      <span></span>
    </div>
  );
};
