'use client';

import classNames from 'classnames';
import React from 'react';
import styles from './AllocationTable.module.scss';

export interface IOptionItem {
  label: string;
  value: string;
}

export interface DropdownProps {
  /** Label for dropdown */
  label: string;
  /** Options to display */
  options: IOptionItem[];
  /** Which dropdown is currently active */
  active?: string | null;
  /** Event fires when the option changes */
  onChange: (option: IOptionItem) => void;
}

export const Dropdown: React.FC<DropdownProps> = ({
  label,
  options,
  active: initActive = null,
  onChange,
}) => {
  const [active, setActive] = React.useState<string | null>(initActive);

  React.useEffect(() => {
    setActive(initActive);
  }, [initActive]);

  return (
    <div
      className={classNames(styles.sortDropdown, {
        [styles.visible]: active === label,
      })}
      tabIndex={0}
      onClick={() => setActive(active ? null : label)}
      onBlur={() => setActive(null)}
    >
      <p>{label}</p>
      <div className={styles.dropdown}>
        <ul>
          {options.map((option) => (
            <li key={option.value} onClick={() => onChange(option)}>
              {option.label}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};
