import { uniqueId } from 'lodash';
import moment from 'moment';
import React from 'react';
import styles from './DateRangePicker.module.scss';

export interface IDateRangePickerProps {
  /** Input field ID */
  id?: string;
  /** Input name */
  name?: string;
  /** The start and end date initial values */
  values?: string[];
  /** Class name */
  className?: string;
  /** Event fires when date range changes */
  onChange?: (values: string[], e: React.ChangeEvent<HTMLInputElement>) => void;
}

export const DateRangePicker = ({
  id = uniqueId(),
  name = '',
  values: initValues = ['', ''],
  className,
  onChange,
}: IDateRangePickerProps) => {
  // Always make sure there are two values in the array.
  const values = [
    initValues.length > 0 ? initValues[0] : '',
    initValues.length > 1 ? initValues[1] : '',
  ];
  const [selected, setSelected] = React.useState<string[]>(values);

  React.useEffect(() => {
    setSelected([values[0], values[1]]);
    // If we use 'values' it will do an infinite loop.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [values[0], values[1]]);

  return (
    <div className={`${styles.dateRange} ${className}`}>
      <div className={styles.datePicker}>
        <label htmlFor={`${id}-startDate`}>Start Date:</label>
        <input
          type="date"
          id={`${id}-startDate`}
          name={`${name}-startDate`}
          value={selected[0] ? moment(selected[0]).format('YYYY-MM-DD') : ''}
          onChange={(e) => {
            if (onChange) {
              const result = [moment(e.target.value).format('YYYY-MM-DD 00:00:00'), values[1]];
              onChange?.(result, e);
            } else {
              setSelected((values) => [
                moment(e.target.value).format('YYYY-MM-DD 00:00:00'),
                values[1],
              ]);
            }
          }}
        />
      </div>
      <div className={styles.datePicker}>
        <label htmlFor={`${id}-endDate`}>End Date:</label>
        <input
          type="date"
          id={`${id}-endDate`}
          name={`${name}-endDate`}
          value={selected[1] ? moment(selected[1]).format('YYYY-MM-DD') : ''}
          onChange={(e) => {
            if (onChange) {
              const result = [values[0], moment(e.target.value).format('YYYY-MM-DD 23:59:59')];
              onChange?.(result, e);
            } else {
              setSelected((values) => [
                values[0],
                moment(e.target.value).format('YYYY-MM-DD 23:59:59'),
              ]);
            }
          }}
        />
      </div>
    </div>
  );
};
