'use client';

import { Button } from '@/components';
import { Checkbox } from '@/components/forms/checkbox';
import { Select } from '@/components/forms/select';
import styles from './StyleGuide.module.scss';

export default function Page() {
  return (
    <div className={`container ${styles.styleGuide}`}>
      <div className={`panel ${styles.panel}`}>
        <h1 className={styles.heading}>Welcome to the Storage Dashboard Style Guide</h1>
        <div>
          <h2 className={styles.heading}>Colours</h2>
          <div className={styles.colours}>
            <div></div>
            <div></div>
            <div></div>
            <div></div>
          </div>
        </div>
        <div>
          <h2 className={styles.heading}>Typography</h2>
          <ul className={styles.list}>
            <li><h1>Main Heading</h1></li>
            <li><h2>Sub-heading</h2></li>
            <li><p>Paragraph text</p></li>
          </ul>
        </div>
        <div className={styles.buttons}>
          <h2 className={styles.heading}>Buttons</h2>
          <Button title="Primary Button" variant="primary">
            Button primary
          </Button>
          <Button title="Secondary Button" variant="secondary">
            Button secondary
          </Button>
        </div>
        <div>
          <h2 className={styles.heading}>Checkbox</h2>
          <Checkbox></Checkbox>
        </div>
        <div>
          <h2 className={styles.heading}>Toggle</h2>
          <Checkbox></Checkbox>
        </div>
        <div>
          <h2 className={styles.heading}>Select</h2>
          <Select title="dropdown example"></Select>
        </div>
        <div>
          <h2 className={styles.heading}>Inputs</h2>
        </div>
      </div>
    </div>
  );
}
