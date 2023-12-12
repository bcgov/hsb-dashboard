'use client';

import styles from './Filter.module.scss';
import { Button } from '@/components/buttons';

export const Filter: React.FC = () => {

  return (
    <div className={styles.filter}>
      <h1>Filter</h1>
      <div className={styles.dropdown}>
        <label htmlFor="organization">Organization</label>
        <select id="organization">
          <option value="" selected disabled>
            Select Organization
          </option>
          <option value="value1">Option 1</option>
          <option value="value2">Option 2</option>
          <option value="value3">Option 3</option>
          <option value="value4">Option 4</option>
        </select>
      </div>
      <div className={styles.dropdown}>
        <label htmlFor="os">Operating system</label>
        <select id="os">
          <option value="" selected disabled>
            Select OS
          </option>
          <option value="value1">Option 1</option>
          <option value="value2">Option 2</option>
          <option value="value3">Option 3</option>
          <option value="value4">Option 4</option>
        </select>
      </div>
      <div className={styles.dropdown}>
        <label htmlFor="server">Server</label>
        <select id="server">
          <option value="" selected disabled>
            Select server
          </option>
          <option value="value1">Option 1</option>
          <option value="value2">Option 2</option>
          <option value="value3">Option 3</option>
          <option value="value4">Option 4</option>
        </select>
      </div>
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
