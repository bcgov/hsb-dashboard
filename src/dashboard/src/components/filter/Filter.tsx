'use client';

import { Button, useDashboardFilter } from '@/components';
import { IOperatingSystemItemModel, IOrganizationModel, ITenantModel } from '@/hooks';
import { useDashboardServerHistoryItems } from '@/hooks/dashboard';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/data';
import { useFilteredServerItems } from '@/hooks/filter';
import { useFiltered } from '@/store';
import moment from 'moment';
import { useSearchParams } from 'next/navigation';
import React from 'react';
import styles from './Filter.module.scss';
import { FilteredOperatingSystemItems } from './FilteredOperatingSystemItems';
import { FilteredOrganizations } from './FilteredOrganizations';
import { FilteredServerItems } from './FilteredServerItems';
import { FilteredTenants } from './FilteredTenants';
import { useUrlParamsUpdateKey } from './hooks';

export const Filter: React.FC = () => {
  const params = useSearchParams();
  const { isReady: tenantsReady, tenants } = useTenants();
  const { isReady: organizationsReady, organizations } = useOrganizations();
  const { isReady: operatingSystemItemsReady, operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady, serverItems } = useServerItems({
    useSimple: true,
  });

  const filteredDateRange = useFiltered((state) => state.dateRange);
  const setFilteredDateRange = useFiltered((state) => state.setDateRange);

  const { findServerItems } = useFilteredServerItems({
    useSimple: true,
  });

  const { isReady: serverHistoryItemsReady } = useDashboardServerHistoryItems();

  const { readyKey, lockKey } = useUrlParamsUpdateKey();
  const updateDashboard = useDashboardFilter();

  const currentParams = React.useMemo(
    () => new URLSearchParams(Array.from(params.entries())),
    [params],
  );

  // Extract URL query parameters and initialize state.
  const tenantId = currentParams.get('tenant');
  const organizationId = currentParams.get('organization');
  const operatingSystemItemId = currentParams.get('operatingSystemItem');
  const serverItemKey = currentParams.get('serverItem');

  React.useEffect(() => {
    // Create a key to unlock a dashboard update.
    // A key requires all URL query parameters to be ready.
    const tenant = tenantId ? tenants.find((t) => t.id === +tenantId) : undefined;
    const organization = organizationId
      ? organizations.find((t) => t.id === +organizationId)
      : undefined;
    const operatingSystemItem = operatingSystemItemId
      ? operatingSystemItems.find((t) => t.id === +operatingSystemItemId)
      : undefined;
    const serverItem = serverItemKey
      ? serverItems.find((t) => t.serviceNowKey === serverItemKey)
      : undefined;

    if (tenant) readyKey.current = readyKey.current | 1;
    if (organization) readyKey.current = readyKey.current | 2;
    if (operatingSystemItem) readyKey.current = readyKey.current | 4;
    if (serverItem) readyKey.current = readyKey.current | 8;
  }, [
    operatingSystemItemId,
    operatingSystemItems,
    organizationId,
    organizations,
    readyKey,
    serverItemKey,
    serverItems,
    tenantId,
    tenants,
  ]);

  React.useEffect(() => {
    // We only want to update the dashboard once.
    // If we don't have an update key it will attempt to update every time 'anything' changes.
    if (readyKey.current && lockKey === readyKey.current) {
      const tenant = tenantId ? tenants.find((t) => t.id === +tenantId) : undefined;
      const organization = organizationId
        ? organizations.find((t) => t.id === +organizationId)
        : undefined;
      const operatingSystemItem = operatingSystemItemId
        ? operatingSystemItems.find((t) => t.id === +operatingSystemItemId)
        : undefined;
      const serverItem = serverItemKey
        ? serverItems.find((t) => t.serviceNowKey === serverItemKey)
        : undefined;

      readyKey.current = 0; // Destroy key so that it does not update the dashboard again.
      updateDashboard({ tenant, organization, operatingSystemItem, serverItem });
    }
    // We only want to update the dashboard when the URL values change.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [readyKey, updateDashboard, lockKey]);

  React.useEffect(() => {
    if (!filteredDateRange[0]) {
      setFilteredDateRange([
        moment().subtract(1, 'year').format('YYYY-MM-DD 00:00:00'),
        filteredDateRange ? filteredDateRange[1] : '',
      ]);
    }
  }, [filteredDateRange, setFilteredDateRange]);

  const handleFindServerItems = React.useCallback(
    async (
      tenant?: ITenantModel,
      organization?: IOrganizationModel,
      operatingSystemItem?: IOperatingSystemItemModel,
    ) => {
      return await findServerItems({
        tenantId: tenant?.id,
        organizationId: organization?.id,
        operatingSystemItemId: operatingSystemItem?.id,
      });
    },
    [findServerItems],
  );

  return (
    <div className={styles.filter}>
      <h1>Filter by:</h1>
      <FilteredTenants
        onChange={async (tenant, organization, operatingSystem) => {
          return await handleFindServerItems(tenant, organization, operatingSystem);
        }}
      />
      <FilteredOrganizations
        onChange={async (tenant, organization, operatingSystem) => {
          return await handleFindServerItems(tenant, organization, operatingSystem);
        }}
      />
      <FilteredOperatingSystemItems
        onChange={async (tenant, organization, operatingSystem) => {
          return await handleFindServerItems(tenant, organization, operatingSystem);
        }}
      />
      <FilteredServerItems />
      {/* <DateRangePicker
        values={filteredDateRange}
        onChange={async (values, e) => {
          setFilteredDateRange(values);
          const serverItems = await handleFindServerItems(
            filteredTenant,
            filteredOrganization,
            filteredOperatingSystemItem,
          );
          if (serverItems.length === 1) setFilteredServerItem(serverItems[0]);
        }}
      /> */}

      <Button
        variant="primary"
        disabled={
          !tenantsReady ||
          !organizationsReady ||
          !operatingSystemItemsReady ||
          !serverItemsReady ||
          !serverHistoryItemsReady
        }
        loading={!serverHistoryItemsReady}
        onClick={async () => {
          await updateDashboard();
        }}
      >
        Update
      </Button>
      <button
        className={styles.reset}
        onClick={async () => {
          await updateDashboard({ reset: true });
        }}
      >
        Reset Filters
      </button>
    </div>
  );
};
