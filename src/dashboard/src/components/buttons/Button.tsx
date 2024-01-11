import Image from 'next/image';
import React from 'react';
import { Spinner } from '../spinner';
import styles from './Button.module.scss';

interface IButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
  children?: React.ReactNode;
  iconPath?: string;
  loading?: boolean;
}

export const Button: React.FC<IButtonProps> = ({
  variant = 'primary',
  children,
  iconPath,
  disabled,
  loading,
  onKeyDown,
  onClick,
  ...rest
}) => {
  // Determine the button's className based on the 'variant' prop and whether iconPath is provided.
  const getButtonClassName = () => {
    let buttonClasses = `${styles.btn} ${styles[variant] || ''}`;

    if (iconPath) {
      // If iconPath is truthy, append the class for an icon
      buttonClasses += ` ${styles.btnWithIcon}`;
    }

    if (disabled) {
      // If the button is disabled, append the disabled class
      buttonClasses += ` ${styles.disabled}`;
    }

    return buttonClasses;
  };

  return (
    <button
      className={getButtonClassName()}
      disabled={disabled}
      onKeyDown={(e) => {
        if (e.code === 'Space') onClick?.(e as any);
        onKeyDown?.(e);
      }}
      onClick={(e) => {
        onClick?.(e);
      }}
      {...rest}
    >
      {iconPath && (
        <Image src={iconPath} alt="icon" width={17} height={17} className={styles.buttonIcon} />
      )}
      {loading && <Spinner className={styles.spinner} />}
      {children}
    </button>
  );
};
