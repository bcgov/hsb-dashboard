import style from './Row.module.scss';

export interface IRowProps extends React.HTMLAttributes<HTMLDivElement> {}

export const Row: React.FC<IRowProps> = ({ children, className, ...rest }) => {
  return (
    <div className={`${style.row} ${className && className}`} {...rest}>
      {children}
    </div>
  );
};
