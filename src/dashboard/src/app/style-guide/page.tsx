'use client';

import { Button } from '@/components/buttons';
import { Checkbox } from '@/components/forms/checkbox';
import { Select } from '@/components/forms/select';
import { Text } from '@/components/forms/text';
import { Toggle } from '@/components/forms/toggle';
import styles from './StyleGuide.module.scss';

export default function Page() {
  return (
    <div className={styles.styleGuide}>
      <div className={styles.panel}>
        <h1 className={styles.heading}>Welcome to the Storage Dashboard Style Guide</h1>
        <div>
          <h2 className={styles.heading}>Colours</h2>
          <div className={styles.colours}>
            <div data-color="#003366"></div>
            <div data-color="#FCBA19"></div>
            <div data-color="#313132"></div>
            <div data-color="#F2F2F2"></div>
            <div data-color="#949494"></div>
          </div>
        </div>
        <div>
          <h2 className={styles.heading}>Typography</h2>
          <ul className={styles.list}>
            <li>
              <h1>Main Heading</h1>
            </li>
            <li>
              <h2>Sub-heading</h2>
            </li>
            <li>
              <p>Paragraph text</p>
            </li>
            <li>
              <a href="#" className={styles.link}>
                Link text
              </a>
            </li>
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
          <Button title="Secondary Button" variant="secondary" iconPath="/images/trash-icon.png">
            Button with icon
          </Button>
          <Button title="Disabled Button" disabled>
            Disabled button
          </Button>
        </div>
        <div>
          <h2 className={styles.heading}>Checkbox</h2>
          <Checkbox></Checkbox>
        </div>
        <div>
          <h2 className={styles.heading}>Toggle</h2>
          <Toggle></Toggle>
        </div>
        <div>
          <h2 className={styles.heading}>Select Dropdown</h2>
          <Select
            variant="filter"
            title="filter dropdown example"
            options={[]}
            label="Filter dropdown"
            placeholder="Default option"
          ></Select>
          <br />
          <Select
            variant="primary"
            title="simple dropdown example"
            options={[]}
            label="Simple dropdown"
            placeholder="Default option"
          ></Select>
        </div>
        <div>
          <h2 className={styles.heading}>Text Inputs</h2>
          <Text placeholder="Placeholder Text"></Text>
          <Text placeholder="Search icon" iconType={'search'}></Text>
          <Text placeholder="Filter icon" iconType={'filter'}></Text>
        </div>
      </div>
    </div>
  );
}
