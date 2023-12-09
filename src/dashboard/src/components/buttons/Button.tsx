import styles from './buttons.module.scss';

import Image from 'next/image';

interface IButtonProps extends React.HTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary' | 'success' | 'warn' | 'error' | 'info' | 'link';
  children?: React.ReactNode;
  iconPath?: string;
}

export const Button: React.FC<IButtonProps> = ({ variant, children, iconPath, ...rest }) => {
  var style = '';
  if (!variant || variant === 'primary')
    style = 'group bg-blue hover:bg-blue-900 active:bg-blue-950 text-white py-2 px-4 rounded';
  else if (variant === 'secondary')
    style =
      'group bg-white border border-gray-200 hover:bg-gray-100 active:bg-gray-950 text-black py-2 px-4 rounded';
  else if (variant === 'success')
    style = 'group bg-green hover:bg-green-400 active:bg-green-600 text-white py-2 px-4 rounded';
  else if (variant === 'warn')
    style = 'group bg-yellow hover:bg-yellow-400 active:bg-yellow-600 text-black py-2 px-4 rounded';
  else if (variant === 'error')
    style = 'group bg-red hover:bg-red-400 active:bg-red-600 text-white py-2 px-4 rounded';
  else if (variant === 'info')
    style = 'group bg-white hover:bg-gray-400 active:bg-gray-600 text-black py-2 px-4 rounded';
  else if (variant === 'link')
    style = 'group bg-white hover:bg-gray-400 active:bg-gray-600 text-black py-2 px-4 rounded';

  return (
    <button className={`${style}${rest.className ? ` ${rest.className}` : ''}`} {...rest}>
      {iconPath && <Image src={iconPath} alt="icon" className={styles.buttonIcon} />}
      {children}
    </button>
  );
};
