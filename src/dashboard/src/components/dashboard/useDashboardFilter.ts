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
        values?.tenants ?? values?.applyFilter ? filteredTenants : values?.reset ? tenants : [],
      );

      setDashboardOrganization(values?.organization);
      setDashboardOrganizations(
        values?.organizations ?? values?.applyFilter
          ? filteredOrganizations
          : values?.reset
          ? organizations
          : [],
      );

      setDashboardOperatingSystemItem(values?.operatingSystemItem);
      setDashboardOperatingSystemItems(
        values?.operatingSystemItems ?? values?.applyFilter
          ? filteredOperatingSystemItems
          : values?.reset
          ? operatingSystemItems
          : [],
      );

      setDashboardServerItem(values?.serverItem);
      setDashboardServerItems(
        values?.serverItems ?? values?.applyFilter
          ? filteredServerItems
          : values?.reset
          ? serverItems
          : [],
      );

      if (values?.serverItem) {
        await findFileSystemItems({
          serverItemServiceNowKey: values?.serverItem.serviceNowKey,
        });
      }

      // TODO: Because useEffects cannot be awaited this will result in bad behaviour.
      // It would be nice to be able to react to URL parameters and state variables, but both don't work together.
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
