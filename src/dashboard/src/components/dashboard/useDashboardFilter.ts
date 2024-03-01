'use client';

import {
  IOperatingSystemItemListModel,
  IOrganizationListModel,
  IServerItemListModel,
  ITenantListModel,
} from '@/hooks';
import { useFilteredFileSystemItems } from '@/hooks/filter';
import { useAppStore, useDashboardStore, useFilteredStore } from '@/store';
import { useSearchParams } from 'next/navigation';
import React from 'react';

export interface IDashboardValues {
  /** Set the selected tenant. */
  tenant?: ITenantListModel;
  /** Set the selected tenants. */
  tenants?: ITenantListModel[];
  /** Set the selected organization. */
  organization?: IOrganizationListModel;
  /** Set the selected organizations. */
  organizations?: IOrganizationListModel[];
  /** Set the selected operating system item. */
  operatingSystemItem?: IOperatingSystemItemListModel;
  /** Set the selected operating system items. */
  operatingSystemItems?: IOperatingSystemItemListModel[];
  /** Set the selected server item. */
  serverItem?: IServerItemListModel;
  /** Set the selected server items. */
  serverItems?: IServerItemListModel[];
  /** Apply the current filter values to the dashboard. */
  applyFilter?: boolean;
  /** Update all dashboard values with original lists in memory. */
  reset?: boolean;
  /** Whether to make a request for the file system items. Defaults to true. */
  fetchFileSystemItems?: boolean;
}

/**
 * Hook provides a function to update dashboard state.
 * The function will apply the current filter to the dashboard, or will update the filter based on the passed in selected values.
 * @returns Function to update dashboard state.
 */
export const useDashboardFilter = () => {
  const params = useSearchParams();

  const tenants = useAppStore((state) => state.tenants);
  const organizations = useAppStore((state) => state.organizations);
  const operatingSystemItems = useAppStore((state) => state.operatingSystemItems);
  const serverItems = useAppStore((state) => state.serverItems);

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

  const filteredTenants = useFilteredStore((state) => state.tenants);
  const filteredOrganizations = useFilteredStore((state) => state.organizations);
  const filteredOperatingSystemItems = useFilteredStore((state) => state.operatingSystemItems);
  const filteredServerItems = useFilteredStore((state) => state.serverItems);

  const currentParams = React.useMemo(
    () => new URLSearchParams(Array.from(params.entries())),
    [params],
  );

  return React.useCallback(
    async (values?: IDashboardValues) => {
      const options: IDashboardValues = values
        ? {
            ...values,
            fetchFileSystemItems:
              values.fetchFileSystemItems !== undefined ? values.fetchFileSystemItems : true,
          }
        : {};

      // Update the URL
      if (options.tenant) {
        currentParams.set('tenant', options.tenant.id.toString());
      } else {
        currentParams.delete('tenant');
      }
      if (options.organization) {
        currentParams.set('organization', options.organization.id.toString());
      } else {
        currentParams.delete('organization');
      }
      if (options.operatingSystemItem) {
        currentParams.set('operatingSystemItem', options.operatingSystemItem.id.toString());
      } else {
        currentParams.delete('operatingSystemItem');
      }
      if (options.serverItem) {
        currentParams.set('serverItem', options.serverItem.serviceNowKey);
      } else {
        currentParams.delete('serverItem');
      }

      setDashboardTenant(options.tenant);
      setDashboardTenants(
        options.tenants ?? (options.reset ? tenants : options.applyFilter ? filteredTenants : []),
      );

      setDashboardOrganization(options.organization);
      setDashboardOrganizations(
        options.organizations ??
          (options.reset ? organizations : options.applyFilter ? filteredOrganizations : []),
      );

      setDashboardOperatingSystemItem(options.operatingSystemItem);
      setDashboardOperatingSystemItems(
        options.operatingSystemItems ??
          (options.reset
            ? operatingSystemItems
            : options.applyFilter
            ? filteredOperatingSystemItems
            : []),
      );

      setDashboardServerItem(options.serverItem);
      setDashboardServerItems(
        options.serverItems ??
          (options.reset ? serverItems : options.applyFilter ? filteredServerItems : []),
      );

      if (options.serverItem && options.fetchFileSystemItems) {
        await findFileSystemItems({
          serverItemServiceNowKey: options.serverItem.serviceNowKey,
          installStatus: 1,
        });
      }

      // Add parameters to URL.
      // This will cause an infinite loop regrettably.
      // router.push(`${path}?${currentParams.toString()}`);
    },
    [
      currentParams,
      filteredOperatingSystemItems,
      filteredOrganizations,
      filteredServerItems,
      filteredTenants,
      findFileSystemItems,
      operatingSystemItems,
      organizations,
      serverItems,
      setDashboardOperatingSystemItem,
      setDashboardOperatingSystemItems,
      setDashboardOrganization,
      setDashboardOrganizations,
      setDashboardServerItem,
      setDashboardServerItems,
      setDashboardTenant,
      setDashboardTenants,
      tenants,
    ],
  );
};
