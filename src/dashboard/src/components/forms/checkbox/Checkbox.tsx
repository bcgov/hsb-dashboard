import { uniqueId } from 'lodash';
import { ChangeEventHandler, FocusEventHandler } from 'react';
import styles from './Checkbox.module.scss';

export interface ICheckboxProps {
  id?: string;
  name?: string;
  label?: string;
  checked?: boolean;
  value?: string | number | readonly string[];
  className?: string;
  disabled?: boolean;
  onChange?: ChangeEventHandler<HTMLInputElement>;
  onBlur?: FocusEventHandler<HTMLInputElement>;
}

export const Checkbox = ({
  label,
  id = uniqueId(),
  name,
  checked,
  value,
  className = '',
  disabled,
  onChange,
  onBlur,
}: ICheckboxProps) => {
  return (
    <label className={`${styles.container} ${className}`}>
      {label && <label htmlFor={id}>{label}</label>}
      <input
        id={id}
        name={name}
        type="checkbox"
        checked={checked}
        disabled={disabled}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
      />
      <span className={styles.checkmark}></span>
    </label>
  );
};
