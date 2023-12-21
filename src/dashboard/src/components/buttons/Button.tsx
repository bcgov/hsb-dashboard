import React from 'react';
import Image from 'next/image';
import styles from './Buttons.module.scss';

interface IButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'disabled';
  children?: React.ReactNode;
  iconPath?: string;
}

export const Button: React.FC<IButtonProps> = ({ variant = 'primary', children, iconPath, ...rest }) => {
  // Determine the button's className based on the 'variant' prop and whether iconPath is provided.
  const getButtonClassName = () => {
    let buttonClasses = `${styles.btn} ${styles[variant] || styles.primary}`;
    
    if (iconPath) {
      // If iconPath is truthy, append the class for an icon
      buttonClasses += ` ${styles.btnWithIcon}`;
    }

    return buttonClasses;
  };

  return (
    <button
      className={getButtonClassName()}
      // If the variant is 'disabled', add the 'disabled' attribute to the button element
      disabled={variant === 'disabled'}
      {...rest}
    >
      {iconPath && (
        <Image src={iconPath} alt="icon" width={17} height={17} className={styles.buttonIcon} />
      )}
      {children}
    </button>
  );
};
