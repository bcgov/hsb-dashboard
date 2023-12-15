import styles from './Overlay.module.scss';

export interface IOverlayProps {
  children?: React.ReactNode;
}

export const Overlay = ({ children }: IOverlayProps) => {
  return <div className={styles.overlay}>{children}</div>;
};
