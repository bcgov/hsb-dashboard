import React from 'react';
import styles from './Select.module.scss';

interface SelectProps {
  title?: string;
}

export const Select: React.FC<SelectProps> = ({ title = '' }) => {

  const selectRef = React.useRef<HTMLSelectElement>(null);

  const handleChange = () => {
      selectRef.current?.blur();
  };

  return (
    <div className={styles.container}>
      <select title={title} className={styles.select} ref={selectRef} onChange={handleChange}>
        <option>Select</option>
        <option>Option</option>
        <option>Option</option>
      </select>
      <span></span>
    </div>
  );
};
