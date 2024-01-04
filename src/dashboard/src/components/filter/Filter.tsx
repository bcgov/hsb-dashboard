'use client';

import { Button, DateRangePicker, Select } from '@/components';
import { IOperatingSystemItemModel, IOrganizationModel, ITenantModel } from '@/hooks';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/data';
import {
  useFilteredFileSystemItems,
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredServerItems,
  useFilteredTenants,
} from '@/hooks/filter';
import { useFiltered } from '@/store';
import moment from 'moment';
import React from 'react';
import styles from './Filter.module.scss';

export const Filter: React.FC = () => {
  const { tenants } = useTenants();
  const { organizations } = useOrganizations();
  const { operatingSystemItems } = useOperatingSystemItems();
  const { serverItems } = useServerItems();

  const dateRange = useFiltered((state) => state.dateRange);
  const setDateRange = useFiltered((state) => state.setDateRange);

  const tenant = useFiltered((state) => state.tenant);
  const setTenant = useFiltered((state) => state.setTenant);
  const setTenants = useFiltered((state) => state.setTenants);
  const { options: filteredTenantOptions } = useFilteredTenants();

  const organization = useFiltered((state) => state.organization);
  const setOrganization = useFiltered((state) => state.setOrganization);
  const setOrganizations = useFiltered((state) => state.setOrganizations);
  const { options: filteredOrganizationOptions, findOrganizations } = useFilteredOrganizations();

  const operatingSystemItem = useFiltered((state) => state.operatingSystemItem);
  const setOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const setOperatingSystemItems = useFiltered((state) => state.setOperatingSystemItems);
  const { options: filteredOperatingSystemItemOptions, findOperatingSystemItems } =
    useFilteredOperatingSystemItems();

  const serverItem = useFiltered((state) => state.serverItem);
  const setServerItem = useFiltered((state) => state.setServerItem);
  const setServerItems = useFiltered((state) => state.setServerItems);
  const { options: filteredServerItemOptions, findServerItems } = useFilteredServerItems();

  const { findFileSystemItems } = useFilteredFileSystemItems();

  React.useEffect(() => {
    setTenants(tenants);
  }, [setTenants, tenants]);

  React.useEffect(() => {
    setOrganizations(organizations);
  }, [setOrganizations, organizations]);

  React.useEffect(() => {
    setOperatingSystemItems(operatingSystemItems);
  }, [setOperatingSystemItems, operatingSystemItems]);

  React.useEffect(() => {
    setServerItems(serverItems);
  }, [setServerItems, serverItems]);

  React.useEffect(() => {
    if (!dateRange[0]) {
      setDateRange([
        moment().subtract(1, 'year').format('YYYY-MM-DD 00:00:00'),
        dateRange ? dateRange[1] : '',
      ]);
    }
  }, [dateRange, setDateRange]);

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
        value={tenant?.id ?? ''}
        onChange={async (value) => {
          const tenant = tenants.find((t) => t.id == value);
          setTenant(tenant);
          setOrganization();
          setOperatingSystemItem();
          setServerItem();

          if (tenant) {
            const organizations = await findOrganizations({ tenantId: tenant.id });
            if (organizations.length === 1) setOrganization(organizations[0]);

            const operatingSystems = await findOperatingSystemItems({
              tenantId: tenant.id,
              organizationId: organization?.id,
            });
            if (operatingSystems.length === 1) setOperatingSystemItem(operatingSystems[0]);

            const serverItems = await handleFindServerItems(
              tenant,
              organization,
              operatingSystemItem,
            );
            if (serverItems.length === 1) setServerItem(serverItems[0]);
          } else {
            setOrganizations(organizations);
            setOperatingSystemItems(operatingSystemItems);
            await handleFindServerItems(tenant);
          }
        }}
      />
      <Select
        label="Organization"
        variant="filter"
        options={filteredOrganizationOptions}
        placeholder="Select organization"
        value={organization?.id ?? ''}
        onChange={async (value) => {
          const organization = organizations.find((o) => o.id == value);
          setOrganization(organization);
          setOperatingSystemItem();
          setServerItem();

          if (organization) {
            const operatingSystems = await findOperatingSystemItems({
              tenantId: tenant?.id,
              organizationId: organization.id,
            });
            if (operatingSystems.length === 1) setOperatingSystemItem(operatingSystems[0]);

            const serverItems = await handleFindServerItems(
              tenant,
              organization,
              operatingSystemItem,
            );
            if (serverItems.length === 1) setServerItem(serverItems[0]);
          } else {
            setOperatingSystemItems(operatingSystemItems);
            await handleFindServerItems(tenant, organization);
          }
        }}
      />
      <Select
        label="Operating system"
        variant="filter"
        options={filteredOperatingSystemItemOptions}
        placeholder="Select OS"
        value={operatingSystemItem?.id ?? ''}
        onChange={async (value) => {
          const operatingSystemItem = operatingSystemItems.find((o) => o.id == value);
          setOperatingSystemItem(operatingSystemItem);
          setServerItem();

          if (operatingSystemItem) {
            const serverItems = await handleFindServerItems(
              tenant,
              organization,
              operatingSystemItem,
            );
            if (serverItems.length === 1) setServerItem(serverItems[0]);
          } else {
            await handleFindServerItems(tenant, organization, operatingSystemItem);
          }
        }}
      />
      <Select
        label="Server"
        variant="filter"
        options={filteredServerItemOptions}
        placeholder="Select server"
        value={serverItem?.serviceNowKey ?? ''}
        onChange={async (value) => {
          const server = serverItems.find((o) => o.serviceNowKey == value);
          setServerItem(server);
        }}
      />
      <DateRangePicker
        values={dateRange}
        onChange={async (values, e) => {
          setDateRange(values);
          const serverItems = await handleFindServerItems(
            tenant,
            organization,
            operatingSystemItem,
          );
          if (serverItems.length === 1) setServerItem(serverItems[0]);
        }}
      />

      <Button
        variant="primary"
        onClick={async () => {
          await findFileSystemItems({
            startDate: dateRange[0] ? dateRange[0] : undefined,
            endDate: dateRange[1] ? dateRange[1] : undefined,
            tenantId: tenant?.id,
            organizationId: organization?.id,
            operatingSystemItemId: operatingSystemItem?.id,
            serverItemServiceNowKey: serverItem?.serviceNowKey,
          });
        }}
      >
        Update
      </Button>
    </div>
  );
};
