'use client';

import React from 'react';
import classNames from 'classnames';
import styles from './AllocationTable.module.scss';

interface DropdownProps {
  label: string;
  options: string[];
  visibleDropdown: string | null;
  toggleDropdown: (label: string) => void;
}

export const Dropdown: React.FC<DropdownProps> = ({ label, options, visibleDropdown, toggleDropdown }) => (
  <div
    className={classNames(styles.sortDropdown, {
      [styles.visible]: visibleDropdown === label,
    })}
    onClick={() => toggleDropdown(label)}
    tabIndex={0}
  >
    <p>{label}</p>
    <div className={styles.dropdown}>
      <ul>
        {options.map((option, index) => (
          <li key={index}>{option}</li>
        ))}
      </ul>
    </div>
  </div>
);
