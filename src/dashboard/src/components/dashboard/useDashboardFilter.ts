'use client';

import {
  IOperatingSystemItemModel,
  IOrganizationModel,
  IServerItemModel,
  ITenantModel,
} from '@/hooks';
import { useFilteredFileSystemItems } from '@/hooks/filter';
import { useAppStore, useDashboardStore, useFilteredStore } from '@/store';
import { useSearchParams } from 'next/navigation';
import React from 'react';

export interface IDashboardValues {
  tenant?: ITenantModel;
  tenants?: ITenantModel[];
  organization?: IOrganizationModel;
  organizations?: IOrganizationModel[];
  operatingSystemItem?: IOperatingSystemItemModel;
  operatingSystemItems?: IOperatingSystemItemModel[];
  serverItem?: IServerItemModel;
  serverItems?: IServerItemModel[];
  applyFilter?: boolean;
  reset?: boolean;
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
      // Update the URL
      if (values?.tenant) {
        currentParams.set('tenant', values.tenant.id.toString());
      } else {
        currentParams.delete('tenant');
      }
      if (values?.organization) {
        currentParams.set('organization', values.organization.id.toString());
      } else {
        currentParams.delete('organization');
      }
      if (values?.operatingSystemItem) {
        currentParams.set('operatingSystemItem', values.operatingSystemItem.id.toString());
      } else {
        currentParams.delete('operatingSystemItem');
      }
      if (values?.serverItem) {
        currentParams.set('serverItem', values.serverItem.serviceNowKey);
      } else {
        currentParams.delete('serverItem');
      }

      setDashboardTenant(values?.tenant);
      setDashboardTenants(
        values?.tenants ?? (values?.reset ? tenants : values?.applyFilter ? filteredTenants : []),
      );

      setDashboardOrganization(values?.organization);
      setDashboardOrganizations(
        values?.organizations ??
          (values?.reset ? organizations : values?.applyFilter ? filteredOrganizations : []),
      );

      setDashboardOperatingSystemItem(values?.operatingSystemItem);
      setDashboardOperatingSystemItems(
        values?.operatingSystemItems ??
          (values?.reset
            ? operatingSystemItems
            : values?.applyFilter
            ? filteredOperatingSystemItems
            : []),
      );

      setDashboardServerItem(values?.serverItem);
      setDashboardServerItems(
        values?.serverItems ??
          (values?.reset ? serverItems : values?.applyFilter ? filteredServerItems : []),
      );

      if (values?.serverItem) {
        await findFileSystemItems({
          serverItemServiceNowKey: values?.serverItem.serviceNowKey,
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
