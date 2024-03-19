'use client';

import { Button } from '@/components/buttons';
import { Text } from '@/components/forms/text';
import { IServerItemListModel } from '@/hooks';
import { convertToStorageSize } from '@/utils/convertToStorageSize';
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
export interface IAllocationTableProps {
  /** Filter servers by their OS */
  osClassName?: string;
  operatingSystemId?: number;
  serverItems: IServerItemListModel[];
  loading?: boolean;
  margin?: number;
  showExport?: boolean;
  exportDisabled?: boolean;
  onExport?: (search: string) => void;
  onClick?: (serverItem?: IServerItemListModel) => void;
}

export const AllocationTable = ({
  osClassName,
  operatingSystemId,
  serverItems,
  loading,
  margin,
  showExport,
  exportDisabled,
  onExport,
  onClick,
}: IAllocationTableProps) => {
  const getServerItems = useAllocationByOS(osClassName, operatingSystemId);

  const [keyword, setKeyword] = React.useState('');
  const [filter, setFilter] = React.useState(keyword);
  const [sort, setSort] = React.useState<string>('server:asc');
  const [rows, setRows] = React.useState<ITableRowData<IServerItemListModel>[]>([]);
  const [filteredServerItems, setFilteredServerItems] = React.useState<IServerItemListModel[]>([]);
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
      sorting[0] as keyof ITableRowData<IServerItemListModel>,
      sorting[1] as any,
    );
    setRows(rows);
  }, [serverItems, filter, getServerItems, sort]);

  const totalCapacity = rows.reduce((acc, row) => acc + (row.capacity || 0), 0);
  const capacityValue = convertToStorageSize<string>(totalCapacity, 'B', 'TB');

  const totalUnused = rows.reduce((acc, row) => acc + (row.available || 0), 0);
  const unusedValue = convertToStorageSize<string>(totalUnused, 'B', 'TB');

  const totalUsed = totalCapacity - totalUnused;
  const usedValue = convertToStorageSize<string>(totalUsed, 'B', 'TB');

  const showTenants = React.useMemo(() => rows.some((data) => data.tenant), [rows]);
  const columns = React.useMemo(() => getColumns(showTenants), [showTenants]);

  // eslint-disable-next-line react-hooks/exhaustive-deps
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

  const selectFromDropdown = (item: IServerItemListModel) => {
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
  const getCommonOSName = (serverRows: ITableRowData<IServerItemListModel>[]): string | null => {
    if (serverRows.length === 0) return null; // Early exit if no servers

    const firstOS = serverRows[0].os;
    const commonOS = serverRows.every((row) => row.os === firstOS);

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
          : `${serverItems.length.toLocaleString()} Servers ${
              commonOSName ? `using OS: "${commonOSName}"` : ''
            }`}
      </h1>
      <h2>
        Total Allocated: {capacityValue} <span></span> Used: {usedValue} <span></span> Unused:{' '}
        {unusedValue}
      </h2>
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
      {showExport && (
        <Button
          variant="secondary"
          iconPath="/images/download-icon.png"
          disabled={exportDisabled}
          onClick={() => onExport?.(filter)}
        >
          Export to Excel
        </Button>
      )}
    </div>
  );
};
