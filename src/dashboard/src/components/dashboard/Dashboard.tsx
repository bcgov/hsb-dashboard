'use client';

import {
  AllOrganizations,
  AllocationByOS,
  AllocationByStorageVolume,
  AllocationByVolume,
  AllocationTable,
  SegmentedBarChart,
  StorageTrendsChart,
  TotalStorage,
} from '@/components/charts';
import {
  useDashboardOperatingSystemItems,
  useDashboardOrganizations,
  useDashboardServerItems,
} from '@/hooks/dashboard';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/data';
import { useFilteredFileSystemItems } from '@/hooks/filter';
import { useFilteredStore } from '@/store';
import React from 'react';
import { useDashboardFilter } from '.';

/**
 * Dashboard component displays different charts depending on what data has been stored in the dashboard state.
 * The Filter component updates the dashboard state when the Update button is clicked.
 * @returns Component
 */
export const Dashboard = () => {
  const { isReady: isReadyTenants } = useTenants({ init: true });
  const { isReady: isReadyOrganizations } = useOrganizations({
    init: true,
    includeTenants: true,
  });
  const { isReady: isReadyOperatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const { isReady: isReadyServerItems } = useServerItems({
    init: true,
    useSimple: true,
  });
  const { organization: dashboardOrganization, organizations: dashboardOrganizations } =
    useDashboardOrganizations();
  const {
    operatingSystemItem: dashboardOperatingSystemItem,
    operatingSystemItems: dashboardOperatingSystemItems,
  } = useDashboardOperatingSystemItems();
  const { serverItem: dashboardServerItem, serverItems: dashboardServerItems } =
    useDashboardServerItems();
  const { isLoading: fileSystemItemsIsLoading, fileSystemItems } = useFilteredFileSystemItems();
  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);

  const updateDashboard = useDashboardFilter();

  // Total storage is for a single organization
  const showTotalStorage =
    !!dashboardServerItem || (!!dashboardOrganization && !dashboardOperatingSystemItem);
  // All organizations is for multiple organizations
  const showAllOrganizations =
    !dashboardOrganization && !dashboardServerItem && !dashboardOperatingSystemItem;
  // For multiple OS
  const showAllocationByOS =
    !!dashboardOrganization && !dashboardOperatingSystemItem && !dashboardServerItem;
  // A single server
  const showAllocationByVolume = !!dashboardServerItem;
  // All servers within available organizations
  const showAllocationByStorageVolume = !dashboardOrganization && !dashboardServerItem;
  // All servers with OS
  const showAllocationTable = !!dashboardOperatingSystemItem && !dashboardServerItem;
  // Show each drive over time for server
  const showSegmentedBarChart = !!dashboardServerItem;

  React.useEffect(() => {
    // When no filter is selected use all values available.
    if (
      isReadyTenants &&
      !values.tenant &&
      isReadyOrganizations &&
      !values.organization &&
      isReadyOperatingSystemItems &&
      !values.operatingSystemItem &&
      isReadyServerItems &&
      !values.serverItem
    ) {
      updateDashboard({ reset: true });
    }
  }, [
    isReadyOperatingSystemItems,
    isReadyOrganizations,
    isReadyServerItems,
    isReadyTenants,
    updateDashboard,
    values.operatingSystemItem,
    values.organization,
    values.serverItem,
    values.tenant,
  ]);

  return (
    <>
      {/* Single Organization total storage */}
      {showTotalStorage && (
        <TotalStorage
          serverItems={dashboardServerItem ? [dashboardServerItem] : dashboardServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
        />
      )}
      {/* Multiple OS */}
      {showAllocationByOS && (
        <AllocationByOS
          operatingSystemItems={dashboardOperatingSystemItems}
          serverItems={dashboardServerItems}
          loading={!isReadyOperatingSystemItems || !isReadyServerItems}
          onClick={async (operatingSystemItem) => {
            setValues((state) => ({ operatingSystemItem }));
            await updateDashboard({ operatingSystemItem });
          }}
        />
      )}
      {/* One Server Selected */}
      {showAllocationByVolume && (
        <AllocationByVolume fileSystemItems={fileSystemItems} loading={fileSystemItemsIsLoading} />
      )}
      {/* Multiple Organizations */}
      {showAllOrganizations && (
        <AllOrganizations
          organizations={dashboardOrganizations}
          serverItems={dashboardServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
        />
      )}
      <StorageTrendsChart
        large={!!dashboardOrganization || !!dashboardOperatingSystemItem || !!dashboardServerItem}
      />
      {showAllocationByStorageVolume && (
        <AllocationByStorageVolume
          organizations={dashboardOrganizations}
          serverItems={dashboardServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
          onClick={async (organization) => {
            setValues((state) => ({ organization }));
            await updateDashboard({ organization });
          }}
        />
      )}
      {showAllocationTable && (
        <AllocationTable
          operatingSystemId={dashboardOperatingSystemItem?.id}
          serverItems={dashboardServerItems}
          loading={!isReadyServerItems || !isReadyOperatingSystemItems}
          onClick={async (serverItem) => {
            setValues((state) => ({ serverItem }));
            await updateDashboard({ serverItem });
          }}
        />
      )}
      {showSegmentedBarChart && (
        <SegmentedBarChart serverItem={dashboardServerItem} loading={!isReadyServerItems} />
      )}
    </>
  );
};
