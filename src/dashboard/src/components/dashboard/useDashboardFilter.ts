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
} from '@/hooks/filter';
import { useAppStore, useDashboardStore, useFilteredStore } from '@/store';
import { useSearchParams } from 'next/navigation';
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

  const filteredTenant = useFilteredStore((state) => state.tenant);
  const setFilteredTenant = useFilteredStore((state) => state.setTenant);
  const setFilteredTenants = useFilteredStore((state) => state.setTenants);
  const filteredOrganization = useFilteredStore((state) => state.organization);
  const setFilteredOrganization = useFilteredStore((state) => state.setOrganization);
  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const { organizations: filteredOrganizations } = useFilteredOrganizations();
  const filteredOperatingSystemItem = useFilteredStore((state) => state.operatingSystemItem);
  const setFilteredOperatingSystemItem = useFilteredStore((state) => state.setOperatingSystemItem);
  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { operatingSystemItems: filteredOperatingSystemItems } = useFilteredOperatingSystemItems();
  const filteredServerItem = useFilteredStore((state) => state.serverItem);
  const setFilteredServerItem = useFilteredStore((state) => state.setServerItem);
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const filteredServerItems = useFilteredStore((state) => state.serverItems);

  const currentParams = React.useMemo(
    () => new URLSearchParams(Array.from(params.entries())),
    [params],
  );

  return React.useCallback(
    async (filter?: IDashboardFilterProps) => {
      const selectedTenant = filter?.reset ? undefined : filter?.tenant ?? filteredTenant;
      const selectedOrganization = filter?.reset
        ? undefined
        : filter?.organization ?? filteredOrganization;
      const selectedOperatingSystemItem = filter?.reset
        ? undefined
        : filter?.operatingSystemItem ?? filteredOperatingSystemItem;
      const selectedServerItem = filter?.reset
        ? undefined
        : filter?.serverItem ?? filteredServerItem;

      if (filter?.reset || filter?.tenant) {
        setFilteredTenant(filter.tenant);
      }
      if (filter?.reset || filter?.organization) {
        setFilteredOrganization(filter.organization);
      }
      if (filter?.reset || filter?.operatingSystemItem) {
        setFilteredOperatingSystemItem(filter.operatingSystemItem);
      }
      if (filter?.reset || filter?.serverItem) {
        setFilteredServerItem(filter.serverItem);
      }

      // Update the URL
      if (selectedTenant) {
        currentParams.set('tenant', selectedTenant.id.toString());
      } else {
        currentParams.delete('tenant');
      }
      if (selectedOrganization) {
        currentParams.set('organization', selectedOrganization.id.toString());
      } else {
        currentParams.delete('organization');
      }
      if (selectedOperatingSystemItem) {
        currentParams.set('operatingSystemItem', selectedOperatingSystemItem.id.toString());
      } else {
        currentParams.delete('operatingSystemItem');
      }
      if (selectedServerItem) {
        currentParams.set('serverItem', selectedServerItem.serviceNowKey);
      } else {
        currentParams.delete('serverItem');
      }

      if (selectedTenant) {
        setDashboardTenant(selectedTenant);
        setDashboardTenants([selectedTenant]);
      } else {
        setDashboardTenant(undefined);
        setFilteredTenants(tenants);
        setDashboardTenants(tenants);
      }

      if (selectedOrganization) {
        setDashboardOrganization(selectedOrganization);
        setDashboardOrganizations([selectedOrganization]);
      } else {
        setDashboardOrganization(undefined);
        if (selectedTenant) setDashboardOrganizations(filteredOrganizations);
        else {
          setFilteredOrganizations(organizations);
          setDashboardOrganizations(organizations);
        }
      }

      if (selectedOperatingSystemItem) {
        setDashboardOperatingSystemItem(selectedOperatingSystemItem);
        setDashboardOperatingSystemItems([selectedOperatingSystemItem]);
      } else {
        setDashboardOperatingSystemItem(undefined);
        if (selectedOrganization) setDashboardOperatingSystemItems(filteredOperatingSystemItems);
        else {
          setFilteredOperatingSystemItems(operatingSystemItems);
          setDashboardOperatingSystemItems(operatingSystemItems);
        }
      }

      if (selectedServerItem) {
        setDashboardServerItem(selectedServerItem);
        setDashboardServerItems([selectedServerItem]);
      } else {
        setDashboardServerItem(undefined);
        if (selectedOperatingSystemItem) setDashboardServerItems(filteredServerItems);
        else {
          setFilteredServerItems(serverItems);
          setDashboardServerItems(serverItems);
        }
      }

      if (selectedServerItem) {
        await findFileSystemItems({
          serverItemServiceNowKey: selectedServerItem.serviceNowKey,
        });
      }

      // TODO: Because useEffects cannot be awaited this will result in bad behaviour.
      // It would be nice to be able to react to URL parameters and state variables, but both don't work together.
      // router.push(`${path}?${currentParams.toString()}`);
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
      setFilteredOperatingSystemItem,
      setFilteredOperatingSystemItems,
      setFilteredOrganization,
      setFilteredOrganizations,
      setFilteredServerItem,
      setFilteredServerItems,
      setFilteredTenant,
      setFilteredTenants,
      tenants,
    ],
  );
};
