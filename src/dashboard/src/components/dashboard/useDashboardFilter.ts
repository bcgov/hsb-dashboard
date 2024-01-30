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
import { usePathname, useRouter, useSearchParams } from 'next/navigation';
import React from 'react';

export interface IDashboardFilterProps {
  tenant?: ITenantModel;
  organization?: IOrganizationModel;
  operatingSystemItem?: IOperatingSystemItemModel;
  serverItem?: IServerItemModel;
  reset?: boolean;
}

/**
 * Hook provides a function to update dashboard state.
 * The function will apply the current filter to the dashboard, or will update the filter based on the passed in selected values.
 * @returns Function to update dashboard state.
 */
export const useDashboardFilter = () => {
  const router = useRouter();
  const path = usePathname();
  const params = useSearchParams();
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

  const currentParams = React.useMemo(
    () => new URLSearchParams(Array.from(params.entries())),
    [params],
  );

  return React.useCallback(
    async (filter?: IDashboardFilterProps) => {
      const selectedTenant = filter?.reset ? undefined : filter?.tenant ?? filteredTenant;
      if (filter?.tenant) setFilteredTenant(filter.tenant);
      if (selectedTenant) {
        currentParams.set('tenant', selectedTenant.id.toString());
        setDashboardTenants([selectedTenant]);
      } else {
        currentParams.delete('tenant');
        setFilteredTenant(undefined);
        setDashboardTenants(filteredTenants);
      }

      const selectedOrganization = filter?.reset
        ? undefined
        : filter?.organization ?? filteredOrganization;
      if (filter?.reset || filter?.organization) setFilteredOrganization(filter.organization);
      if (selectedOrganization) {
        currentParams.set('organization', selectedOrganization.id.toString());
        setDashboardOrganizations([selectedOrganization]);
      } else {
        currentParams.delete('organization');
        setFilteredOrganization(undefined);
        setDashboardOrganizations(filteredOrganizations);
      }

      const selectedOperatingSystemItem = filter?.reset
        ? undefined
        : filter?.operatingSystemItem ?? filteredOperatingSystemItem;
      if (filter?.reset || filter?.operatingSystemItem)
        setFilteredOperatingSystemItem(filter.operatingSystemItem);
      if (selectedOperatingSystemItem) {
        currentParams.set('operatingSystemItem', selectedOperatingSystemItem.id.toString());
        setDashboardOperatingSystemItems([selectedOperatingSystemItem]);
      } else {
        currentParams.delete('operatingSystemItem');
        setFilteredOperatingSystemItem(undefined);
        setDashboardOperatingSystemItems(filteredOperatingSystemItems);
      }

      const selectedServerItem = filter?.reset
        ? undefined
        : filter?.serverItem ?? filteredServerItem;
      if (filter?.reset || filter?.serverItem) setFilteredServerItem(filter.serverItem);
      if (selectedServerItem) {
        currentParams.set('serverItem', selectedServerItem.serviceNowKey);
        setDashboardServerItems([selectedServerItem]);
      } else {
        currentParams.delete('serverItem');
        setFilteredServerItem(undefined);
        setDashboardServerItems(filteredServerItems);
      }

      setDashboardDateRange(filteredDateRange);
      router.push(`${path}?${currentParams.toString()}`);

      if (selectedServerItem) {
        await findFileSystemItems({
          serverItemServiceNowKey: selectedServerItem.serviceNowKey,
        });
      }

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
      currentParams,
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
      path,
      router,
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
