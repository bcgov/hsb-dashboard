import { Spinner } from '@/components';
import classNames from 'classnames';
import { uniqueId } from 'lodash';
import React, { FocusEventHandler } from 'react';
import { FormError, IOption } from '..';
import styles from './Select.module.scss';
import { generateKey } from './utils';

export interface FilterDropdownProps<T> {
  options: IOption<T>[];
  label?: string;
  title?: string;
  id?: string;
  name?: string;
  value?: string | number | readonly string[];
  disabled?: boolean;
  placeholder?: string;
  error?: React.ReactNode;
  loading?: boolean;
  onChange?: (
    values: string | number | readonly string[] | undefined,
    event?: React.FormEvent<HTMLSelectElement>,
  ) => void;
  onBlur?: FocusEventHandler<HTMLSelectElement>;
}

export const FilterDropdown = <T extends unknown>({
  options,
  label,
  title,
  id = uniqueId(),
  name,
  value,
  disabled,
  placeholder,
  error,
  loading,
  onChange,
  onBlur,
}: FilterDropdownProps<T>) => {
  const [selected, setSelected] = React.useState<string | number | readonly string[] | undefined>(
    value,
  );
  const [selectedLabel, setSelectedLabel] = React.useState<string | undefined>(undefined);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [filteredOptions, setFilteredOptions] = React.useState(options);
  const [isOpen, setIsOpen] = React.useState(false);
  const wrapperRef = React.useRef<HTMLDivElement>(null);
  const inputRef = React.useRef<HTMLInputElement>(null);

  React.useEffect(() => {
    setSelected(value);
  }, [value]);

  React.useEffect(() => {
    const selectedOption = options.find((option) => option.value === selected);
    setSelectedLabel(typeof selectedOption?.label === 'string' ? selectedOption.label : undefined);
  }, [selected, options]);

  React.useEffect(() => {
    const lowercasedFilter = searchTerm.toLowerCase();
    const filteredData = options.filter(
      (item) =>
        typeof item.label === 'string' && item.label.toLowerCase().includes(lowercasedFilter),
    );
    setFilteredOptions(filteredData);
  }, [options, searchTerm]);

  React.useEffect(() => {
    function handleClickOutside(e: MouseEvent) {
      if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
        setIsOpen(false);
      }
    }

    if (isOpen && inputRef.current) {
      inputRef.current.focus();
    }

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [isOpen, wrapperRef]);

  // Function to handle selection change
  const handleSelectChange = (value: string | number, optionLabel: string) => {
    setSelected(value);
    setSelectedLabel(optionLabel);
    onChange?.(value);
    setSearchTerm(''); // Clear the search box upon selection
    setIsOpen(false); // Close the dropdown upon selection
  };

  return (
    <div className={styles.dropdown} ref={wrapperRef}>
      {label && <label htmlFor={id}>{label}</label>}
      {loading && <Spinner className={styles.spinner} />}

      <div className={styles.filterDropdown} title={title} onClick={() => setIsOpen(!isOpen)}>
        <p title={selectedLabel && selectedLabel}>{selectedLabel ? selectedLabel : placeholder}</p>
        {isOpen && (
          <ul className={styles.dropdownList}>
            <li>
              <input
                type="text"
                id={id}
                name={name}
                value={searchTerm}
                disabled={disabled}
                placeholder={'Search list'}
                ref={inputRef}
                onClick={(e) => e.stopPropagation()}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </li>
            {filteredOptions.map((option) => (
              <li
                key={generateKey(option.value)}
                className={classNames({
                  [styles.selected]:
                    selected === option.value ||
                    (Array.isArray(selected) && selected.includes(option.value)),
                })}
                onClick={() =>
                  (typeof option.value === 'string' || typeof option.value === 'number') &&
                  typeof option.label === 'string'
                    ? handleSelectChange(option.value, option.label ?? '')
                    : null
                }
              >
                {option.label}
              </li>
            ))}
          </ul>
        )}
      </div>
      <FormError message={error} />
    </div>
  );
};
