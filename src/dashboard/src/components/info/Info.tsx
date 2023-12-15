import { FaInfoCircle } from 'react-icons/fa';
import styles from './Info.module.scss';

export interface IInfoProps {
  icon?: React.ReactNode;
  children?: React.ReactNode;
}

export const Info = ({ icon = <FaInfoCircle />, children }: IInfoProps) => {
  return (
    <p className={styles.info}>
      {icon}
      {children}
    </p>
  );
};
