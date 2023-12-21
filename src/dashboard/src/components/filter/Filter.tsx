'use client';

import { Select } from '@/components';
import { Button } from '@/components/buttons';
import { useOperatingSystemItems, useOrganizations, useServerItems, useTenants } from '@/hooks';
import React from 'react';
import styles from './Filter.module.scss';

export const Filter: React.FC = () => {
  const { options: tenantOptions } = useTenants();
  const { options: organizationOptions } = useOrganizations();
  const { options: operatingSystemItemOptions } = useOperatingSystemItems();
  const { options: serverItemOptions } = useServerItems();

  return (
    <div className={styles.filter}>
      <h1>Filter</h1>
      <Select options={tenantOptions} label="Tenant" placeholder="Select tenant" />
      <Select
        options={organizationOptions}
        label="Organization"
        placeholder="Select organization"
      />
      <Select
        options={operatingSystemItemOptions}
        label="Operating system"
        placeholder="Select OS"
      />
      <Select options={serverItemOptions} label="Server" placeholder="Select server" />
      <div className={styles.date}>
        <div className={styles.datePicker}>
          <label htmlFor="endDate">Start Date:</label>
          <input type="date" id="startDate" />
        </div>
        <div className={styles.datePicker}>
          <label htmlFor="endDate">End Date:</label>
          <input type="date" id="endDate" />
        </div>
      </div>

      <Button variant="primary">Update</Button>
    </div>
  );
};
