interface IButtonProps extends React.HTMLAttributes<HTMLButtonElement> {
  children?: React.ReactNode;
}

export const Button: React.FC<IButtonProps> = ({ children, ...rest }) => {
  return <button {...rest}>{children}</button>;
};
