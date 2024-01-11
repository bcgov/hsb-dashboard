'use client';

import { Button } from '@/components/buttons';
import { Text } from '@/components/forms/text';
import { useServerItems } from '@/hooks/data';
import classNames from 'classnames';
import { debounce } from 'lodash';
import React from 'react';
import styles from './AllocationTable.module.scss';
import { Dropdown } from './Dropdown';
import { ITableRowData } from './ITableRowData';
import { TableRow } from './TableRow';
import { OperatingSystems } from './constants';
import { useAllocationByOS } from './hooks';
import { getColumns, getLabel } from './utils';

export interface IAllocationTableProps {
  /** Filter servers by their OS */
  operatingSystem: OperatingSystems;
}

export const AllocationTable = ({ operatingSystem }: IAllocationTableProps) => {
  const { serverItems } = useServerItems();
  const getServerItems = useAllocationByOS(operatingSystem);

  const [keyword, setKeyword] = React.useState('');
  const [filter, setFilter] = React.useState(keyword);
  const [sort, setSort] = React.useState<string>('server:asc');
  const [rows, setRows] = React.useState<ITableRowData[]>([]);

  React.useEffect(() => {
    const sorting = sort.split(':');
    const rows = getServerItems(
      serverItems,
      (si) =>
        !filter ||
        si.name.toLocaleLowerCase().includes(filter.toLocaleLowerCase()) ||
        (!!si.operatingSystemItem &&
          si.operatingSystemItem.name.toLocaleLowerCase().includes(filter.toLocaleLowerCase())),
      sorting[0] as keyof ITableRowData,
      sorting[1] as any,
    );
    setRows(rows);
  }, [serverItems, filter, getServerItems, sort]);

  const showTenants = React.useMemo(() => rows.some((data) => data.tenant), [rows]);
  const columns = React.useMemo(() => getColumns(showTenants), [showTenants]);

  const debounceChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => debounce(() => setKeyword(e.target.value), 500),
    [],
  );

  return (
    <div className={styles.panel}>
      <h1>Allocation by Storage Volume - All {getLabel(operatingSystem)}</h1>
      <div className={styles.filter}>
        <Text
          placeholder="Filter by server name, OS version"
          iconType={'filter'}
          onChange={(e) => debounceChange(e)()}
          onKeyDown={(e) => {
            if (e.code === 'Enter') setFilter(keyword);
          }}
        />
        <Button variant="secondary" onClick={() => setFilter(keyword)}>
          Apply
        </Button>
      </div>
      <div className={classNames(styles.tableContainer, { [styles.hasTenant]: showTenants })}>
        <div className={styles.header}>
          {columns.map((dropdown) => (
            <Dropdown
              key={dropdown.label}
              label={dropdown.label}
              options={dropdown.options}
              onChange={(option) => setSort(`${dropdown.sort}:${option.value}`)}
            />
          ))}
          <p>Total</p>
        </div>
        <div className={styles.chart}>
          {rows.map((data, index) => (
            <TableRow
              key={index}
              server={data.server}
              tenant={data.tenant}
              os={data.os}
              capacity={data.capacity}
              available={data.available}
              showTenant={showTenants}
            />
          ))}
        </div>
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png" disabled>
        Export to Excel
      </Button>
    </div>
  );
};
