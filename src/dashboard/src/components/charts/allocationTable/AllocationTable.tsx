'use client';

import { Button } from '@/components/buttons';
import { Text } from '@/components/forms/text';
import { IServerItemModel } from '@/hooks';
import classNames from 'classnames';
import { debounce } from 'lodash';
import React from 'react';
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
  const wrapperRef = React.useRef(null);

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

  const showTenants = React.useMemo(() => rows.some((data) => data.tenant), [rows]);
  const columns = React.useMemo(() => getColumns(showTenants), [showTenants]);

  const debounceChange = React.useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => debounce(() => setKeyword(e.target.value), 500),
    [],
  );

  const debouncedSearch = React.useMemo(() => debounce((searchKeyword) => {
    const filtered = serverItems.filter(si =>
      si.name.toLowerCase().includes(searchKeyword.toLowerCase()) ||
      (!!si.operatingSystemItem &&
        si.operatingSystemItem.name.toLowerCase().includes(searchKeyword.toLowerCase()))
    );
    
    // Set the filtered list for the dropdown
    setFilteredServerItems(filtered);
    setShowDropdown(true);
  }, 300), [serverItems]);

  React.useEffect(() => {
    // Make sure to cancel the debounced event when the component is unmounted
    return () => {
      debouncedSearch.cancel();
    };
  }, [debouncedSearch]);

  const handleFilterChange = (e) => {
    const value = e.target.value;
    setKeyword(value);
    debouncedSearch(value);
  };

  const selectFromDropdown = (item: IServerItemModel) => {
    setKeyword(item.name);
    setFilter(item.name);
    setShowDropdown(false); // Close the dropdown after selection
  };

  const handleClickOutside = (e) => {
    if (wrapperRef.current && !wrapperRef.current.contains(e.target)) {
      setShowDropdown(false);
    }
  };
  
  React.useEffect(() => {
    // Bind the event listener
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      // Unbind the event listener on clean up
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  return (
    <div className={styles.panel} style={margin ? { marginTop: margin } : {}}>
      {loading && <LoadingAnimation />}
      <h1>
        {osClassName
          ? `Allocation by Storage Volume - All ${getLabel(osClassName)}`
          : 'All Servers'}
      </h1>
      <div className={styles.filter} ref={wrapperRef}>
        <Text
          placeholder="Filter by server name, OS version"
          iconType={'filter'}
          value={keyword}
          onChange={handleFilterChange}
          onKeyDown={(e) => {
            if (e.code === 'Enter') {
              setFilter(keyword);
              setShowDropdown(false);
            }
          }}
        />
        {showDropdown && (
          <div className={styles.dropdownMenu}>
            {filteredServerItems.map((item, index) => (
              <div
                key={index}
                className={styles.dropdownItem}
                onClick={() => selectFromDropdown(item)}
              >
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
