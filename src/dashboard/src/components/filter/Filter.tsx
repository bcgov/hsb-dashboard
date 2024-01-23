'use client';

import { Button, DateRangePicker, Select, useDashboardFilter } from '@/components';
import { IOperatingSystemItemModel, IOrganizationModel, ITenantModel, useAuth } from '@/hooks';
import { useDashboardServerHistoryItems } from '@/hooks/dashboard';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/data';
import {
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredServerItems,
  useFilteredTenants,
} from '@/hooks/filter';
import { useFiltered } from '@/store';
import moment from 'moment';
import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import React from 'react';
import styles from './Filter.module.scss';
import { useUrlParamsUpdateKey } from './hooks';

export const Filter: React.FC = () => {
  const router = useRouter();
  const path = usePathname();
  const params = useSearchParams();
  const { isHSB } = useAuth();
  const { isReady: tenantsReady, tenants } = useTenants({ init: true });
  const { isReady: organizationsReady, organizations } = useOrganizations({ init: true });
  const { isReady: operatingSystemItemsReady, operatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const { isReady: serverItemsReady, serverItems } = useServerItems({
    useSimple: true,
    init: true,
  });

  const filteredDateRange = useFiltered((state) => state.dateRange);
  const setFilteredDateRange = useFiltered((state) => state.setDateRange);

  const filteredTenant = useFiltered((state) => state.tenant);
  const setFilteredTenant = useFiltered((state) => state.setTenant);
  const setFilteredTenants = useFiltered((state) => state.setTenants);
  const { options: filteredTenantOptions } = useFilteredTenants();

  const filteredOrganization = useFiltered((state) => state.organization);
  const setFilteredOrganization = useFiltered((state) => state.setOrganization);
  const setFilteredOrganizations = useFiltered((state) => state.setOrganizations);
  const { options: filteredOrganizationOptions, findOrganizations } = useFilteredOrganizations();

  const filteredOperatingSystemItem = useFiltered((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const setFilteredOperatingSystemItems = useFiltered((state) => state.setOperatingSystemItems);
  const { options: filteredOperatingSystemItemOptions, findOperatingSystemItems } =
    useFilteredOperatingSystemItems();

  const filteredServerItem = useFiltered((state) => state.serverItem);
  const setFilteredServerItem = useFiltered((state) => state.setServerItem);
  const setFilteredServerItems = useFiltered((state) => state.setServerItems);
  const { options: filteredServerItemOptions, findServerItems } = useFilteredServerItems({
    useSimple: true,
  });

  const { isReady: serverHistoryItemsReady } = useDashboardServerHistoryItems();

  const { readyKey, lockKey } = useUrlParamsUpdateKey();
  const updateDashboard = useDashboardFilter();

  const currentParams = React.useMemo(
    () => new URLSearchParams(Array.from(params.entries())),
    [params],
  );

  const enableTenants = isHSB || tenants.length > 1;
  const enableOrganizations = isHSB || organizations.length > 1;

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
    if (tenants.length === 1) setFilteredTenant(tenants[0]);
    if (tenants.length) setFilteredTenants(tenants);
  }, [setFilteredTenant, setFilteredTenants, tenants]);

  React.useEffect(() => {
    if (organizations.length === 1) setFilteredOrganization(organizations[0]);
    if (organizations.length) setFilteredOrganizations(organizations);
  }, [setFilteredOrganizations, organizations, setFilteredOrganization]);

  React.useEffect(() => {
    if (operatingSystemItems.length === 1) setFilteredOperatingSystemItem(operatingSystemItems[0]);
    if (operatingSystemItems.length) setFilteredOperatingSystemItems(operatingSystemItems);
  }, [setFilteredOperatingSystemItems, operatingSystemItems, setFilteredOperatingSystemItem]);

  React.useEffect(() => {
    if (serverItems.length === 1) setFilteredServerItem(serverItems[0]);
    if (serverItems.length) setFilteredServerItems(serverItems);
  }, [setFilteredServerItems, serverItems, setFilteredServerItem]);

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
      <h1>Filter</h1>
      <Select
        label="Tenant"
        variant="filter"
        options={filteredTenantOptions}
        placeholder="Select tenant"
        value={filteredTenant?.id ?? ''}
        disabled={!tenantsReady || !enableTenants || !serverItemsReady}
        loading={!tenantsReady}
        onChange={async (value) => {
          const tenant = tenants.find((t) => t.id == value);
          setFilteredTenant(tenant);
          setFilteredOrganization();
          setFilteredOperatingSystemItem();
          setFilteredServerItem();

          if (tenant) {
            const organizations = await findOrganizations({ tenantId: tenant.id });
            if (organizations.length === 1) setFilteredOrganization(organizations[0]);

            const operatingSystems = await findOperatingSystemItems({
              tenantId: tenant.id,
              organizationId: filteredOrganization?.id,
            });
            if (operatingSystems.length === 1) setFilteredOperatingSystemItem(operatingSystems[0]);

            const serverItems = await handleFindServerItems(
              tenant,
              filteredOrganization,
              filteredOperatingSystemItem,
            );
            if (serverItems.length === 1) setFilteredServerItem(serverItems[0]);
          } else {
            setFilteredOrganizations(organizations);
            setFilteredOperatingSystemItems(operatingSystemItems);
            await handleFindServerItems(tenant);
          }
        }}
      />
      <Select
        label="Organization"
        variant="filter"
        options={filteredOrganizationOptions}
        placeholder="Select organization"
        value={filteredOrganization?.id ?? ''}
        disabled={!organizationsReady || !enableOrganizations || !serverItemsReady}
        loading={!organizationsReady}
        onChange={async (value) => {
          const organization = organizations.find((o) => o.id == value);
          setFilteredOrganization(organization);
          setFilteredOperatingSystemItem();
          setFilteredServerItem();

          if (organization) {
            const operatingSystems = await findOperatingSystemItems({
              tenantId: filteredTenant?.id,
              organizationId: organization.id,
            });
            if (operatingSystems.length === 1) setFilteredOperatingSystemItem(operatingSystems[0]);

            const serverItems = await handleFindServerItems(
              filteredTenant,
              organization,
              filteredOperatingSystemItem,
            );
            if (serverItems.length === 1) setFilteredServerItem(serverItems[0]);
          } else {
            setFilteredOperatingSystemItems(operatingSystemItems);
            await handleFindServerItems(filteredTenant, organization);
          }
        }}
      />
      <Select
        label="Operating system"
        variant="filter"
        options={filteredOperatingSystemItemOptions}
        placeholder="Select OS"
        value={filteredOperatingSystemItem?.id ?? ''}
        disabled={!operatingSystemItemsReady || !serverItemsReady}
        loading={!operatingSystemItemsReady}
        onChange={async (value) => {
          const operatingSystemItem = operatingSystemItems.find((o) => o.id == value);
          setFilteredOperatingSystemItem(operatingSystemItem);
          setFilteredServerItem();

          if (operatingSystemItem) {
            const serverItems = await handleFindServerItems(
              filteredTenant,
              filteredOrganization,
              operatingSystemItem,
            );
            if (serverItems.length === 1) setFilteredServerItem(serverItems[0]);
          } else {
            await handleFindServerItems(filteredTenant, filteredOrganization, operatingSystemItem);
          }
        }}
      />
      <Select
        label="Server"
        variant="filter"
        options={filteredServerItemOptions}
        placeholder="Select server"
        value={filteredServerItem?.serviceNowKey ?? ''}
        disabled={!serverItemsReady}
        loading={!serverItemsReady}
        onChange={async (value) => {
          const server = serverItems.find((o) => o.serviceNowKey == value);
          setFilteredServerItem(server);
        }}
      />
      <DateRangePicker
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
      />

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
    </div>
  );
};
