import styles from './Toggle.module.scss';

export const Toggle: React.FC = () => {
  return (
    <div className={styles.toggleSwitch}>
      <input type="checkbox" id="toggle" className={styles.toggleSwitchCheckbox} />
      <label className={styles.toggleSwitchLabel} htmlFor="toggle">
        <span className={styles.toggleSwitchInner}></span>
        <span className={styles.toggleSwitchSwitch}></span>
      </label>
    </div>
  );
};
