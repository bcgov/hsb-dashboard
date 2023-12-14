import styles from './Sheet.module.scss';

export interface ISheetProps {
  children?: React.ReactNode;
}

export const Sheet = ({ children }: ISheetProps) => {
  return <div className={styles.sheet}>{children}</div>;
};
