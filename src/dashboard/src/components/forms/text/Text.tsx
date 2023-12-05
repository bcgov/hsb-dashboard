import styles from './Text.module.scss';

type IconType = 'search' | 'filter' | null; // Defines the possible icon types

interface TextProps {
  placeholder: string;
  iconType?: IconType; // Property to determine which icon type to show
}

export const Text: React.FC<TextProps> = ({ placeholder, iconType = null }) => {
  // Determine the appropriate class based on iconType
  let iconClass = '';
  if (iconType === 'search') {
    iconClass = styles.searchIcon;
  } else if (iconType === 'filter') {
    iconClass = styles.filterIcon;
  }

  const className = `${styles.textInput} ${iconClass}`.trim();

  return <input placeholder={placeholder} className={className} />;
};
