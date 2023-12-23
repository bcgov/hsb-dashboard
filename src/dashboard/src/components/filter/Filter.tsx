'use client';

import { Button, DateRangePicker, Select } from '@/components';
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
import React from 'react';
import styles from './Filter.module.scss';

export const Filter: React.FC = () => {
  const tenants = useTenants();
  const organizations = useOrganizations();
  const operatingSystemItems = useOperatingSystemItems();
  const serverItems = useServerItems();

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

  return (
    <div className={styles.filter}>
      <h1>Filter</h1>
      <Select
        variant="filter"
        options={filteredTenantOptions}
        label="Tenant"
        placeholder="Select tenant"
        value={tenant?.id}
        onChange={async (e) => {
          const tenant = tenants.find((t) => t.id === +e.target.value);
          setTenant(tenant);
          if (tenant) {
            await findOrganizations({ tenantId: tenant.id });

            if (organization)
              await findOperatingSystemItems({
                tenantId: tenant.id,
                organizationId: organization.id,
              });
            else await findOperatingSystemItems({ tenantId: tenant.id });

            if (operatingSystemItem)
              await findServerItems({
                tenantId: tenant.id,
                operatingSystemItemId: operatingSystemItem.id,
              });
            else if (organization)
              await findServerItems({ tenantId: tenant.id, organizationId: organization.id });
            else await findServerItems({ tenantId: tenant.id });
          } else {
            setOrganizations(organizations);
            if (!organization) setOperatingSystemItems(operatingSystemItems);
            if (!operatingSystemItem) setServerItems(serverItems);
          }
        }}
      />
      <Select
        variant="filter"
        options={filteredOrganizationOptions}
        label="Organization"
        placeholder="Select organization"
        value={organization?.id}
        onChange={async (e) => {
          const organization = organizations.find((o) => o.id === +e.target.value);
          setOrganization(organization);
          if (organization) {
            await findOperatingSystemItems({ organizationId: organization.id });

            if (operatingSystemItem)
              await findServerItems({
                organizationId: organization.id,
                operatingSystemItemId: operatingSystemItem.id,
              });
            else await findServerItems({ organizationId: organization.id });
          } else {
            setOperatingSystemItems(operatingSystemItems);
            setServerItems(serverItems);
          }
        }}
      />
      <Select
        variant="filter"
        options={filteredOperatingSystemItemOptions}
        label="Operating system"
        placeholder="Select OS"
        value={operatingSystemItem?.id}
        onChange={async (e) => {
          const os = operatingSystemItems.find((o) => o.id === +e.target.value);
          setOperatingSystemItem(os);
          if (os) {
            await findServerItems({ distinct: true, operatingSystemItemId: os.id });
          } else {
            setServerItems(serverItems);
          }
        }}
      />
      <Select
        variant="filter"
        options={filteredServerItemOptions}
        label="Server"
        placeholder="Select server"
        value={serverItem?.id}
        onChange={async (e) => {
          const server = serverItems.find((o) => o.id === +e.target.value);
          setServerItem(server);
        }}
      />
      <DateRangePicker
        values={dateRange}
        onChange={(values, e) => {
          setDateRange(values);
        }}
      />

      <Button
        variant="primary"
        onClick={async () => {
          await findFileSystemItems({
            startDate: dateRange?.[0] ? dateRange[0] : undefined,
            endDate: dateRange?.[1] ? dateRange[1] : undefined,
            tenantId: tenant?.id,
            organizationId: organization?.id,
            operatingSystemItemId: operatingSystemItem?.id,
            serverItemId: serverItem?.id,
          });
        }}
      >
        Update
      </Button>
    </div>
  );
};
