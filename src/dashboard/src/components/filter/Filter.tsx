'use client';

import { Button, useDashboardFilter } from '@/components';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/lists';
import { useFilteredStore } from '@/store';
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
  const { isReady: serverItemsReady, serverItems } = useServerItems();

  const loading = useFilteredStore((state) => state.loading);
  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);

  const setFilteredTenants = useFilteredStore((state) => state.setTenants);
  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);

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

  // React.useEffect(() => {
  //   // Create a key to unlock a dashboard update.
  //   // A key requires all URL query parameters to be ready.
  //   const tenant = tenantId ? tenants.find((t) => t.id === +tenantId) : undefined;
  //   const organization = organizationId
  //     ? organizations.find((t) => t.id === +organizationId)
  //     : undefined;
  //   const operatingSystemItem = operatingSystemItemId
  //     ? operatingSystemItems.find((t) => t.id === +operatingSystemItemId)
  //     : undefined;
  //   const serverItem = serverItemKey
  //     ? serverItems.find((t) => t.serviceNowKey === serverItemKey)
  //     : undefined;

  //   if (tenant) readyKey.current = readyKey.current | 1;
  //   if (organization) readyKey.current = readyKey.current | 2;
  //   if (operatingSystemItem) readyKey.current = readyKey.current | 4;
  //   if (serverItem) readyKey.current = readyKey.current | 8;
  // }, [
  //   operatingSystemItemId,
  //   operatingSystemItems,
  //   organizationId,
  //   organizations,
  //   readyKey,
  //   serverItemKey,
  //   serverItems,
  //   tenantId,
  //   tenants,
  // ]);

  // React.useEffect(() => {
  //   // We only want to update the dashboard once.
  //   // If we don't have an update key it will attempt to update every time 'anything' changes.
  //   if (readyKey.current && lockKey === readyKey.current) {
  //     console.debug('update dashboard');
  //     const tenant = tenantId ? tenants.find((t) => t.id === +tenantId) : undefined;
  //     const organization = organizationId
  //       ? organizations.find((t) => t.id === +organizationId)
  //       : undefined;
  //     const operatingSystemItem = operatingSystemItemId
  //       ? operatingSystemItems.find((t) => t.id === +operatingSystemItemId)
  //       : undefined;
  //     const serverItem = serverItemKey
  //       ? serverItems.find((t) => t.serviceNowKey === serverItemKey)
  //       : undefined;
  //     readyKey.current = 0; // Destroy key so that it does not update the dashboard again.
  //     updateDashboard({ tenant, organization, operatingSystemItem, serverItem });
  //   }
  //   // We only want to update the dashboard when the URL values change.
  //   // eslint-disable-next-line react-hooks/exhaustive-deps
  // }, [readyKey, updateDashboard, lockKey]);

  const allFiltersUnchosen =
    !values.tenant && !values.organization && !values.operatingSystemItem && !values.serverItem;

  return (
    <div className={styles.filter}>
      <h1>Filter by:</h1>
      <FilteredTenants />
      <FilteredOrganizations />
      <FilteredOperatingSystemItems />
      <FilteredServerItems />
      <Button
        variant="primary"
        disabled={
          !tenantsReady ||
          !organizationsReady ||
          !operatingSystemItemsReady ||
          !serverItemsReady ||
          loading
        }
        loading={
          !tenantsReady || !organizationsReady || !operatingSystemItemsReady || !serverItemsReady
        }
        onClick={async () => {
          await updateDashboard({
            tenant: values.tenant,
            organization: values.organization,
            operatingSystemItem: values.operatingSystemItem,
            serverItem: values.serverItem,
            applyFilter: true,
          });
        }}
      >
        Update
      </Button>
      <button
        className={`${styles.reset} ${allFiltersUnchosen ? styles.hidden : ''}`}
        onClick={async () => {
          const tenant = tenants.length === 1 ? tenants[0] : undefined;
          const organization = organizations.length === 1 ? organizations[0] : undefined;
          const operatingSystemItem =
            operatingSystemItems.length === 1 ? operatingSystemItems[0] : undefined;
          const serverItem = serverItems.length === 1 ? serverItems[0] : undefined;
          setValues(() => ({ tenant, organization, operatingSystemItem, serverItem }));
          setFilteredTenants(tenants);
          setFilteredOrganizations(organizations);
          setFilteredOperatingSystemItems(operatingSystemItems);
          setFilteredServerItems(serverItems);
          await updateDashboard({
            tenant,
            organization,
            operatingSystemItem,
            serverItem,
            tenants,
            organizations,
            operatingSystemItems,
            serverItems,
          });
        }}
      >
        Reset Filters
      </button>
    </div>
  );
};
