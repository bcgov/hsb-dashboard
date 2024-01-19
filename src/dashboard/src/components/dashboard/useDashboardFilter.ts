'use client';

import {
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks';
import { useDashboardServerHistoryItems } from '@/hooks/dashboard';
import {
  useFilteredFileSystemItems,
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredTenants,
} from '@/hooks/filter';
import { useDashboard, useFiltered } from '@/store';
import React from 'react';

export interface IDashboardFilterProps {
  tenant?: ITenantModel;
  organization?: IOrganizationModel;
  operatingSystemItem?: IOperatingSystemItemModel;
  serverItem?: IServerItemModel;
}

/**
 * Hook provides a function to update dashboard state.
 * The function will apply the current filter to the dashboard, or will update the filter based on the passed in selected values.
 * @returns Function to update dashboard state.
 */
export const useDashboardFilter = () => {
  const setDashboardTenants = useDashboard((state) => state.setTenants);
  const setDashboardOrganizations = useDashboard((state) => state.setOrganizations);
  const setDashboardOperatingSystemItems = useDashboard((state) => state.setOperatingSystemItems);
  const setDashboardServerItems = useDashboard((state) => state.setServerItems);
  const setDashboardDateRange = useDashboard((state) => state.setDateRange);
  const { findFileSystemItems } = useFilteredFileSystemItems();
  const { findServerHistoryItems } = useDashboardServerHistoryItems();

  const filteredTenant = useFiltered((state) => state.tenant);
  const setFilteredTenant = useFiltered((state) => state.setTenant);
  const { tenants: filteredTenants } = useFilteredTenants();
  const filteredOrganization = useFiltered((state) => state.organization);
  const setFilteredOrganization = useFiltered((state) => state.setOrganization);
  const { organizations: filteredOrganizations } = useFilteredOrganizations();
  const filteredOperatingSystemItem = useFiltered((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFiltered((state) => state.setOperatingSystemItem);
  const { operatingSystemItems: filteredOperatingSystemItems } = useFilteredOperatingSystemItems();
  const filteredServerItem = useFiltered((state) => state.serverItem);
  const setFilteredServerItem = useFiltered((state) => state.setServerItem);
  const filteredServerItems = useFiltered((state) => state.serverItems);
  const filteredDateRange = useFiltered((state) => state.dateRange);

  return React.useCallback(
    async (filter?: IDashboardFilterProps) => {
      const selectedTenant = filter?.tenant ?? filteredTenant;
      const selectedOrganization = filter?.organization ?? filteredOrganization;
      const selectedOperatingSystemItem =
        filter?.operatingSystemItem ?? filteredOperatingSystemItem;
      const selectedServerItem = filter?.serverItem ?? filteredServerItem;

      if (filter?.tenant) setFilteredTenant(filter.tenant);
      if (selectedTenant) setDashboardTenants([selectedTenant]);
      else setDashboardTenants(filteredTenants);

      if (filter?.organization) setFilteredOrganization(filter.organization);
      if (selectedOrganization) setDashboardOrganizations([selectedOrganization]);
      else setDashboardOrganizations(filteredOrganizations);

      if (filter?.operatingSystemItem) setFilteredOperatingSystemItem(filter.operatingSystemItem);
      if (selectedOperatingSystemItem)
        setDashboardOperatingSystemItems([selectedOperatingSystemItem]);
      else setDashboardOperatingSystemItems(filteredOperatingSystemItems);

      if (filter?.serverItem) setFilteredServerItem(filter.serverItem);
      if (selectedServerItem) setDashboardServerItems([selectedServerItem]);
      else setDashboardServerItems(filteredServerItems);

      setDashboardDateRange(filteredDateRange);

      if (selectedServerItem)
        await findFileSystemItems({
          serverItemServiceNowKey: selectedServerItem.serviceNowKey,
        });

      await findServerHistoryItems({
        startDate: filteredDateRange[0] ? filteredDateRange[0] : undefined,
        endDate: filteredDateRange[1] ? filteredDateRange[1] : undefined,
        tenantId: filteredTenant?.id,
        organizationId: filteredOrganization?.id,
        operatingSystemItemId: filteredOperatingSystemItem?.id,
        serviceNowKey: filteredServerItem?.serviceNowKey,
      });
    },
    [
      filteredDateRange,
      filteredOperatingSystemItem,
      filteredOperatingSystemItems,
      filteredOrganization,
      filteredOrganizations,
      filteredServerItem,
      filteredServerItems,
      filteredTenant,
      filteredTenants,
      findFileSystemItems,
      findServerHistoryItems,
      setDashboardDateRange,
      setDashboardOperatingSystemItems,
      setDashboardOrganizations,
      setDashboardServerItems,
      setDashboardTenants,
      setFilteredOperatingSystemItem,
      setFilteredOrganization,
      setFilteredServerItem,
      setFilteredTenant,
    ],
  );
};
