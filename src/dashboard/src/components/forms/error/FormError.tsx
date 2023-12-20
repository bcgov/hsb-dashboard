export interface IErrorProps {
  message: React.ReactNode;
}

export const FormError = ({ message }: IErrorProps) => {
  if (!message) return null;

  if (typeof message === 'string') return <span>{message}</span>;

  return message;
};
