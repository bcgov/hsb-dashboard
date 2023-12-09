import style from './Col.module.scss';

export interface IColProps extends React.HTMLAttributes<HTMLDivElement> {}

export const Col: React.FC<IColProps> = ({ children, className, ...rest }) => {
  return (
    <div className={`${style.col} ${className && className}`} {...rest}>
      {children}
    </div>
  );
};
