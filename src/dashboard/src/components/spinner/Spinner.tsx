import styles from './Spinner.module.scss';

export interface ISpinnerProps {
  className?: string;
}

export const Spinner = ({ className = 'spinner' }: ISpinnerProps) => {
  return (
    <div className={`${styles.spinner} ${className}`}>
      <div></div>
      <div></div>
      <div></div>
      <div></div>
    </div>
  );
};
