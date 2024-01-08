'use client';

import { Button, DateRangePicker, Select } from '@/components';
import { IOperatingSystemItemModel, IOrganizationModel, ITenantModel } from '@/hooks';
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
import { useDashboard, useFiltered } from '@/store';
import moment from 'moment';
import React from 'react';
import styles from './Filter.module.scss';

export const Filter: React.FC = () => {
  const { isReady: tenantsReady, tenants } = useTenants();
  const { isReady: organizationsReady, organizations } = useOrganizations();
  const { isReady: operatingSystemItemsReady, operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady, serverItems } = useServerItems();

  const filteredDateRange = useFiltered((state) => state.dateRange);
  const setFilteredDateRange = useFiltered((state) => state.setDateRange);

  const filteredTenant = useFiltered((state) => state.tenant);
  const setFilteredTenant = useFiltered((state) => state.setTenant);
  const setFilteredTenants = useFiltered((state) => state.setTenants);
  const { options: filteredTenantOptions } = useFilteredTenants();

  const filteredOrganization = useFiltered((state) => state.organization);
  const setFilteredOrganization = useFiltered((state) => state.setOrganization);
  const setFilteredOrganizations = useFiltered((state) => state.setOrganizations);
  const {
    options: filteredOrganizationOptions,
    findOrganizations,
    organizations: filteredOrganizations,
  } = useFilteredOrganizations();

  const filteredOperatingSystemItem = useFiltered((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const setFilteredOperatingSystemItems = useFiltered((state) => state.setOperatingSystemItems);
  const { options: filteredOperatingSystemItemOptions, findOperatingSystemItems } =
    useFilteredOperatingSystemItems();

  const filteredServerItem = useFiltered((state) => state.serverItem);
  const filteredServerItems = useFiltered((state) => state.serverItems);
  const setFilteredServerItem = useFiltered((state) => state.setServerItem);
  const setFilteredServerItems = useFiltered((state) => state.setServerItems);
  const { options: filteredServerItemOptions, findServerItems } = useFilteredServerItems();

  const setDashboardDateRange = useDashboard((state) => state.setDateRange);
  const setDashboardOrganizations = useDashboard((state) => state.setOrganizations);
  const setDashboardServerItems = useDashboard((state) => state.setServerItems);
  const { isReady: serverHistoryItemsReady, findServerHistoryItems } =
    useDashboardServerHistoryItems();

  React.useEffect(() => {
    setFilteredTenants(tenants);
  }, [setFilteredTenants, tenants]);

  React.useEffect(() => {
    setFilteredOrganizations(organizations);
  }, [setFilteredOrganizations, organizations]);

  React.useEffect(() => {
    setFilteredOperatingSystemItems(operatingSystemItems);
  }, [setFilteredOperatingSystemItems, operatingSystemItems]);

  React.useEffect(() => {
    setFilteredServerItems(serverItems);
  }, [setFilteredServerItems, serverItems]);

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
        disabled={!tenantsReady}
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
        disabled={!organizationsReady}
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
        disabled={!operatingSystemItemsReady}
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
          if (filteredOrganization) setDashboardOrganizations([filteredOrganization]);
          else setDashboardOrganizations(filteredOrganizations);

          if (filteredServerItem) setDashboardServerItems([filteredServerItem]);
          else setDashboardServerItems(filteredServerItems);

          setDashboardDateRange(filteredDateRange);

          await findServerHistoryItems({
            startDate: filteredDateRange[0] ? filteredDateRange[0] : undefined,
            endDate: filteredDateRange[1] ? filteredDateRange[1] : undefined,
            tenantId: filteredTenant?.id,
            organizationId: filteredOrganization?.id,
            operatingSystemItemId: filteredOperatingSystemItem?.id,
            serviceNowKey: filteredServerItem?.serviceNowKey,
          });
        }}
      >
        Update
      </Button>
    </div>
  );
};
