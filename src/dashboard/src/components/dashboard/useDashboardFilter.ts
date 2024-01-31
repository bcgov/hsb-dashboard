'use client';

import {
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks';
import {
  useFilteredFileSystemItems,
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredTenants,
} from '@/hooks/filter';
import { useDashboardStore, useFilteredStore } from '@/store';
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

  const setDashboardTenant = useDashboardStore((state) => state.setTenant);
  const setDashboardTenants = useDashboardStore((state) => state.setTenants);

  const setDashboardOrganization = useDashboardStore((state) => state.setOrganization);
  const setDashboardOrganizations = useDashboardStore((state) => state.setOrganizations);

  const setDashboardOperatingSystemItem = useDashboardStore(
    (state) => state.setOperatingSystemItem,
  );
  const setDashboardOperatingSystemItems = useDashboardStore(
    (state) => state.setOperatingSystemItems,
  );

  const setDashboardServerItem = useDashboardStore((state) => state.setServerItem);
  const setDashboardServerItems = useDashboardStore((state) => state.setServerItems);

  const { findFileSystemItems } = useFilteredFileSystemItems();

  const filteredTenant = useFilteredStore((state) => state.tenant);
  const setFilteredTenant = useFilteredStore((state) => state.setTenant);
  const { tenants: filteredTenants } = useFilteredTenants();
  const filteredOrganization = useFilteredStore((state) => state.organization);
  const setFilteredOrganization = useFilteredStore((state) => state.setOrganization);
  const { organizations: filteredOrganizations } = useFilteredOrganizations();
  const filteredOperatingSystemItem = useFilteredStore((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFilteredStore((state) => state.setOperatingSystemItem);
  const { operatingSystemItems: filteredOperatingSystemItems } = useFilteredOperatingSystemItems();
  const filteredServerItem = useFilteredStore((state) => state.serverItem);
  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);
  const filteredServerItems = useFilteredStore((state) => state.serverItems);

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
        setDashboardTenant(selectedTenant);
        setDashboardTenants([selectedTenant]);
      } else {
        currentParams.delete('tenant');
        setDashboardTenant(undefined);
        setFilteredTenant(undefined);
        setDashboardTenants(filteredTenants);
      }

      const selectedOrganization = filter?.reset
        ? undefined
        : filter?.organization ?? filteredOrganization;
      if (filter?.reset || filter?.organization) setFilteredOrganization(filter.organization);
      if (selectedOrganization) {
        currentParams.set('organization', selectedOrganization.id.toString());
        setDashboardOrganization(selectedOrganization);
        setDashboardOrganizations([selectedOrganization]);
      } else {
        currentParams.delete('organization');
        setDashboardOrganization(undefined);
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
        setDashboardOperatingSystemItem(selectedOperatingSystemItem);
        setDashboardOperatingSystemItems([selectedOperatingSystemItem]);
      } else {
        currentParams.delete('operatingSystemItem');
        setDashboardOperatingSystemItem(undefined);
        setFilteredOperatingSystemItem(undefined);
        setDashboardOperatingSystemItems(filteredOperatingSystemItems);
      }

      const selectedServerItem = filter?.reset
        ? undefined
        : filter?.serverItem ?? filteredServerItem;
      if (filter?.reset || filter?.serverItem) setFilteredServerItem(filter.serverItem);
      if (selectedServerItem) {
        currentParams.set('serverItem', selectedServerItem.serviceNowKey);
        setDashboardServerItem(selectedServerItem);
        setDashboardServerItems([selectedServerItem]);
      } else {
        currentParams.delete('serverItem');
        setDashboardServerItem(undefined);
        setFilteredServerItem(undefined);
        setDashboardServerItems(filteredServerItems);
      }

      router.push(`${path}?${currentParams.toString()}`);

      if (selectedServerItem) {
        await findFileSystemItems({
          serverItemServiceNowKey: selectedServerItem.serviceNowKey,
        });
      }
    },
    [
      currentParams,
      filteredOperatingSystemItem,
      filteredOperatingSystemItems,
      filteredOrganization,
      filteredOrganizations,
      filteredServerItem,
      filteredServerItems,
      filteredTenant,
      filteredTenants,
      findFileSystemItems,
      path,
      router,
      setDashboardOperatingSystemItem,
      setDashboardOperatingSystemItems,
      setDashboardOrganization,
      setDashboardOrganizations,
      setDashboardServerItem,
      setDashboardServerItems,
      setDashboardTenant,
      setDashboardTenants,
      setFilteredOperatingSystemItem,
      setFilteredOrganization,
      setFilteredServerItem,
      setFilteredTenant,
    ],
  );
};
