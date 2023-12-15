import { ChangeEventHandler, FocusEventHandler, KeyboardEventHandler } from 'react';
import styles from './Text.module.scss';

interface TextProps extends React.InputHTMLAttributes<HTMLInputElement> {
  /** Include a label */
  label?: string;
  /** Input id */
  id?: string;
  /** Input name */
  name?: string;
  /** Placeholder text */
  placeholder?: string;
  /** Icon type */
  iconType?: 'search' | 'filter'; // Property to determine which icon type to show
  /** Class name */
  className?: string;
  /** Control the input */
  disabled?: boolean;
  /** Event fires when input changes. */
  onChange?: ChangeEventHandler<HTMLInputElement>;
  //** Event fires when input blurs */
  onBlur?: FocusEventHandler<HTMLInputElement>;
  onKeyDown?: KeyboardEventHandler<HTMLInputElement>;
  onKeyDownCapture?: KeyboardEventHandler<HTMLInputElement>;
}

export const Text: React.FC<TextProps> = ({
  id,
  name,
  label,
  placeholder,
  iconType,
  className = '',
  disabled,
  ...rest
}) => {
  // Determine the appropriate class based on iconType
  let iconClass = '';
  if (iconType === 'search') {
    iconClass = styles.searchIcon;
  } else if (iconType === 'filter') {
    iconClass = styles.filterIcon;
  }

  return (
    <>
      {label && <label htmlFor={id}>{label}</label>}
      <input
        id={id}
        name={name}
        placeholder={placeholder}
        disabled={disabled}
        className={`${styles.textInput} ${iconClass} ${className}`}
        {...rest}
      />
    </>
  );
};
