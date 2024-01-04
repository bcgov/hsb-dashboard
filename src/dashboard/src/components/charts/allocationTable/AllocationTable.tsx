'use client';

import { Button } from '@/components/buttons';
import { Text } from '@/components/forms/text';
import React, { useState, useCallback } from 'react';
import classNames from 'classnames';
import styles from './AllocationTable.module.scss';

const operatingSystem = 'Windows';

interface DropdownProps {
  label: string;
  options: string[];
  visibleDropdown: string | null;
  toggleDropdown: (label: string) => void;
}

const Dropdown: React.FC<DropdownProps> = ({ label, options, visibleDropdown, toggleDropdown }) => (
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

export const AllocationTable: React.FC = () => {
  const [visibleDropdown, setVisibleDropdown] = useState<string | null>(null);

  const toggleDropdown = useCallback((label: string) => {
    setVisibleDropdown((prevVisibleDropdown) => (prevVisibleDropdown === label ? null : label));
  }, []);

  const onBlurHandler = useCallback(() => {
    setVisibleDropdown(null);
  }, []);

  return (
    <div className={styles.panel}>
      <h1>Allocation by Storage Volume - All {operatingSystem} Servers</h1>
      <div className={styles.filter}>
        <Text placeholder="Filter by server name, OS version" iconType={'filter'} />
        <Button variant="secondary">Apply</Button>
      </div>
      <div className={styles.tableContainer}>
        <div className={styles.header} onBlur={onBlurHandler}>
          <Dropdown
            label="Server"
            options={['A to Z', 'Z to A']}
            visibleDropdown={visibleDropdown}
            toggleDropdown={toggleDropdown}
          />
          <Dropdown
            label="OS Version"
            options={['Latest', 'Oldest']}
            visibleDropdown={visibleDropdown}
            toggleDropdown={toggleDropdown}
          />
          <Dropdown
            label="Allocated Space"
            options={['Ascending', 'Descending']}
            visibleDropdown={visibleDropdown}
            toggleDropdown={toggleDropdown}
          />
          <Dropdown
            label="Unused Space"
            options={['Ascending', 'Descending']}
            visibleDropdown={visibleDropdown}
            toggleDropdown={toggleDropdown}
          />
          <p>Total</p>
        </div>
      </div>
      <div className={styles.footer}>
        <p>pagination</p>
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png">
        Export to Excel
      </Button>
    </div>
  );
};
