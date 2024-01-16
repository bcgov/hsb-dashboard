'use client';

import { Button } from '@/components/buttons';
import { Select } from '@/components/forms/select';
import { Text } from '@/components/forms/text';
import { IOrganizationModel, IServerItemModel } from '@/hooks';
import React from 'react';
import styles from './AllocationByStorageVolume.module.scss';
import { BarChart } from './BarChart';
import { IOrganizationStorageModel } from './IOrganizationStorageModel';
import { PercentageLines } from './PercentageLines';
import { calcOrganizationStorage } from './calcOrganizationStorage';
import { SortOptions, sortOptions } from './sortOptions';

export interface IAllocationByStorageVolumeProps {
  organizations: IOrganizationModel[];
  serverItems: IServerItemModel[];
  loading?: boolean;
}

export const AllocationByStorageVolume = ({
  organizations,
  serverItems,
  loading,
}: IAllocationByStorageVolumeProps) => {
  const [sortOption, setSortOption] = React.useState<number>(0);
  const [search, setSearch] = React.useState('');
  const [items, setItems] = React.useState<IOrganizationStorageModel[]>(
    calcOrganizationStorage(organizations, serverItems, sortOption),
  );

  React.useEffect(() => {
    setItems(calcOrganizationStorage(organizations, serverItems, sortOption));
  }, [organizations, serverItems, sortOption]);

  const filterOrganizations = React.useCallback(
    (
      value: string,
      organizations: IOrganizationModel[],
      serverItems: IServerItemModel[],
      sort: SortOptions,
    ) => {
      const orgs = value
        ? organizations.filter((o) =>
            o.name.toLocaleLowerCase().includes(value.toLocaleLowerCase()),
          )
        : organizations;
      setItems(calcOrganizationStorage(orgs, serverItems, sort));
    },
    [],
  );

  return (
    <div className={styles.panel}>
      <h1>Allocation by Storage Volume - All Organizations</h1>
      <div className={styles.sort}>
        <Select
          id="allocation"
          variant="primary"
          title="Sort options"
          options={sortOptions}
          value={sortOption}
          onChange={(value) => {
            if (value) setSortOption(+value);
          }}
        />
        <Text
          placeholder="Search"
          iconType={'search'}
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          onKeyDown={(e) =>
            e.code === 'Enter' &&
            filterOrganizations(search, organizations, serverItems, sortOption)
          }
        />
        <Button
          variant="secondary"
          onClick={() => filterOrganizations(search, organizations, serverItems, sortOption)}
        >
          Search
        </Button>
      </div>
      <div className={styles.chartContainer}>
        <PercentageLines />
        {items.map((org) => {
          return (
            <BarChart
              key={org.id}
              to={`/hsb/dashboard/${org.id}`}
              label={org.name}
              availableSpace={org.availableSpace}
              totalStorage={org.capacity}
            />
          );
        })}
      </div>
      <div className={styles.footer}>
        <p>Used</p>
        <p>Unused</p>
      </div>
      <Button variant="secondary" iconPath="/images/download-icon.png" disabled>
        Export to Excel
      </Button>
    </div>
  );
};
