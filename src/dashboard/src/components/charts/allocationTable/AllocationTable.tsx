'use client';

import { Button } from '@/components/buttons';
import { Text } from '@/components/forms/text';
import { IServerItemModel } from '@/hooks';
import classNames from 'classnames';
import { debounce } from 'lodash';
import React, { ChangeEvent } from 'react';
import { LoadingAnimation } from '../../loadingAnimation';
import styles from './AllocationTable.module.scss';
import { Dropdown } from './Dropdown';
import { ITableRowData } from './ITableRowData';
import { TableRow } from './TableRow';
import { useAllocationByOS } from './hooks';
import { getColumns, getLabel } from './utils';
import { convertToStorageSize } from '@/utils/convertToStorageSize';
export interface IAllocationTableProps {
  /** Filter servers by their OS */
  osClassName?: string;
  operatingSystemId?: number;
  serverItems: IServerItemModel[];
  loading?: boolean;
  onClick?: (serverItem?: IServerItemModel) => void;
  margin?: number;
}

export const AllocationTable = ({
  osClassName,
  operatingSystemId,
  serverItems,
  loading,
  onClick,
  margin,
}: IAllocationTableProps) => {
  const getServerItems = useAllocationByOS(osClassName, operatingSystemId);

  const [keyword, setKeyword] = React.useState('');
  const [filter, setFilter] = React.useState(keyword);
  const [sort, setSort] = React.useState<string>('server:asc');
  const [rows, setRows] = React.useState<ITableRowData<IServerItemModel>[]>([]);
  const [filteredServerItems, setFilteredServerItems] = React.useState<IServerItemModel[]>([]);
  const [showDropdown, setShowDropdown] = React.useState(false);
  const wrapperRef = React.useRef<HTMLDivElement>(null);

  React.useEffect(() => {
    const sorting = sort.split(':');
    const rows = getServerItems(
      serverItems,
      (si) =>
        !filter ||
        (si.name.length
          ? si.name.toLocaleLowerCase().includes(filter.toLocaleLowerCase())
          : '[NO NAME]'.toLocaleLowerCase().includes(filter.toLocaleLowerCase())) ||
        (!!si.operatingSystemItem &&
          si.operatingSystemItem.name.toLocaleLowerCase().includes(filter.toLocaleLowerCase())),
      sorting[0] as keyof ITableRowData<IServerItemModel>,
      sorting[1] as any,
    );
    setRows(rows);
  }, [serverItems, filter, getServerItems, sort]);

  const totalCapacity = rows.reduce((acc, row) => acc + (row.capacity || 0), 0);
  const capacityValue = convertToStorageSize<string>(totalCapacity, 'B', 'TB');

  const totalUnused = rows.reduce((acc, row) => acc + (row.available || 0), 0);
  const unusedValue = convertToStorageSize<string>(totalUnused, 'B', 'TB');

  const showTenants = React.useMemo(() => rows.some((data) => data.tenant), [rows]);
  const columns = React.useMemo(() => getColumns(showTenants), [showTenants]);

  const debouncedSearch = React.useCallback(
    debounce((searchKeyword: string) => {
      const filtered = serverItems.filter((si) =>
        si.name.toLowerCase().includes(searchKeyword.toLowerCase()),
      );
      setFilteredServerItems(filtered);
      setShowDropdown(true);
    }, 300),
    [],
  );

  React.useEffect(() => () => debouncedSearch.cancel(), [debouncedSearch]);

  const handleFilterChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setKeyword(value);
    debouncedSearch(value);
  };

  const selectFromDropdown = (item: IServerItemModel) => {
    setKeyword(item.name);
    setFilter(item.name);
    setShowDropdown(false);
  };

  const handleClickOutside = (e: MouseEvent) => {
    if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
      setShowDropdown(false);
    }
  };

  React.useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Check if all servers have the same OS and return that OS name
  const getCommonOSName = (serverRows: ITableRowData<IServerItemModel>[]): string | null => {
    if (serverRows.length === 0) return null; // Early exit if no servers
  
    const firstOS = serverRows[0].os;
    const commonOS = serverRows.every(row => row.os === firstOS);
  
    return commonOS ? firstOS : null;
  };

  const commonOSName = React.useMemo(() => getCommonOSName(rows), [rows]);

  return (
    <div className={styles.panel} style={margin ? { marginTop: margin } : {}}>
      {loading && <LoadingAnimation />}
      <h1>
        {osClassName
          ? `Allocation by Storage Volume - All ${serverItems.length.toLocaleString()} ${getLabel(
              osClassName,
            )}`
          : `${serverItems.length.toLocaleString()} Servers ${commonOSName ? `using OS: "${commonOSName}"` : ''}`}
      </h1>
      <h2>Allocated Storage: {capacityValue} &nbsp;|&nbsp; Unused Storage: {unusedValue}</h2>
      <div className={styles.filter} ref={wrapperRef}>
        <Text
          placeholder="Filter by server name, OS version"
          iconType={'filter'}
          value={keyword}
          onChange={handleFilterChange}
          onKeyDown={(e: React.KeyboardEvent<HTMLInputElement>) => {
            if (e.code === 'Enter') {
              setFilter(keyword);
              setShowDropdown(false);
            }
          }}
        />
        {showDropdown && (
          <div className={styles.filteredDropdown}>
            {filteredServerItems.map((item, index) => (
              <div key={index} onClick={() => selectFromDropdown(item)}>
                {item.name}
              </div>
            ))}
          </div>
        )}
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
          {rows.map((row, index) => (
            <TableRow
              key={index}
              server={row.server}
              tenant={row.tenant}
              os={row.os}
              capacity={row.capacity}
              available={row.available}
              showTenant={showTenants}
              onClick={() => onClick?.(row.data)}
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
