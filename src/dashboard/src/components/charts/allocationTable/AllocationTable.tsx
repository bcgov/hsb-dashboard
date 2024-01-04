'use client';

import { Button } from '@/components/buttons';
import { Text } from '@/components/forms/text';
import React, { useState, useCallback } from 'react';
import styles from './AllocationTable.module.scss';
import classNames from 'classnames';
import { Dropdown } from './Dropdown';
import { TableRow } from './TableRow';
import defaultData from './defaultData';

const operatingSystem = 'Windows';

export const AllocationTable: React.FC = () => {
  const [visibleDropdown, setVisibleDropdown] = useState<string | null>(null);

  const toggleDropdown = useCallback((label: string) => {
    setVisibleDropdown((prevVisibleDropdown) => (prevVisibleDropdown === label ? null : label));
  }, []);

  const onBlurHandler = useCallback(() => {
    setVisibleDropdown(null);
  }, []);

  const hasTenant = defaultData.some(data => data.tenant);

  let dropdownConfigs = [
    { label: "Server", options: ["A to Z", "Z to A"] },
    { label: "OS Version", options: ["Latest", "Oldest"] },
    { label: "Allocated Space", options: ["Ascending", "Descending"] },
    { label: "Unused", options: ["Ascending", "Descending"] }
  ];

  if (hasTenant) {
    dropdownConfigs.splice(1, 0, { label: "Tenant", options: ["A to Z", "Z to A"] });
  }

  return (
    <div className={styles.panel}>
      <h1>Allocation by Storage Volume - All {operatingSystem} Servers</h1>
      <div className={styles.filter}>
        <Text placeholder="Filter by server name, OS version" iconType={'filter'} />
        <Button variant="secondary">Apply</Button>
      </div>
      <div className={classNames(styles.tableContainer, { [styles.hasTenant]: hasTenant })}>
        <div className={styles.header} onBlur={onBlurHandler}>
        {
          dropdownConfigs.map(dropdown => (
            <Dropdown
              key={dropdown.label}
              label={dropdown.label}
              options={dropdown.options}
              visibleDropdown={visibleDropdown}
              toggleDropdown={toggleDropdown}
            />
          ))
        }
          <p>Total</p>
        </div>
        <div className={styles.chart}>
          {defaultData.map((data, index) => (
            <TableRow
              key={index}
              server={data.server}
              tenant={data.tenant}
              os={data.os}
              allocated={data.allocated}
              unused={data.unused}
            />
          ))}
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
