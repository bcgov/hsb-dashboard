import styles from './Checkbox.module.scss';

export const Checkbox: React.FC = () => {
  return (
    <label className={styles.container}>
      <input type="checkbox"/>
      <span className={styles.checkmark}></span>
    </label>
  );
};
