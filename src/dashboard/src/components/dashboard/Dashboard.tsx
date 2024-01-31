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
import { useOperatingSystemItems, useOrganizations, useServerItems } from '@/hooks/data';
import { useFilteredFileSystemItems } from '@/hooks/filter';
import { useDashboardFilter } from '.';

/**
 * Dashboard component displays different charts depending on what data has been stored in the dashboard state.
 * The Filter component updates the dashboard state when the Update button is clicked.
 * @returns Component
 */
export const Dashboard = () => {
  const { isReady: isReadyOrganizations, organizations } = useOrganizations();
  const { isReady: isReadyOperatingSystemItems, operatingSystemItems } = useOperatingSystemItems();
  const { isReady: isReadyServerItems, serverItems } = useServerItems({ useSimple: true });
  const { organization: dashboardOrganization, organizations: dashboardOrganizations } =
    useDashboardOrganizations();
  const {
    operatingSystemItem: dashboardOperatingSystemItem,
    operatingSystemItems: dashboardOperatingSystemItems,
  } = useDashboardOperatingSystemItems();
  const { serverItem: dashboardServerItem, serverItems: dashboardServerItems } =
    useDashboardServerItems();
  const { isLoading: fileSystemItemsIsLoading, fileSystemItems } = useFilteredFileSystemItems();

  const updateDashboard = useDashboardFilter();

  const selectedOrganizations = dashboardOrganizations.length
    ? dashboardOrganizations
    : organizations;
  const selectedOperatingSystemItems = dashboardOperatingSystemItems.length
    ? dashboardOperatingSystemItems
    : operatingSystemItems;
  const selectedServerItems = dashboardServerItems.length ? dashboardServerItems : serverItems;

  // Total storage is for a single organization
  const showTotalStorage =
    !!dashboardServerItem || (!!dashboardOrganization && !dashboardOperatingSystemItem);
  // All organizations is for multiple organizations
  const showAllOrganizations = !dashboardOrganization;
  // For multiple OS
  const showAllocationByOS =
    !!dashboardOrganization && !dashboardOperatingSystemItem && !dashboardServerItem;
  // A single server
  const showAllocationByVolume = !!dashboardServerItem;
  // All servers within available organizations
  const showAllocationByStorageVolume = !dashboardOrganization;
  // All servers with OS
  const showAllocationTable = !!dashboardOperatingSystemItem && !dashboardServerItem;
  // Show each drive over time for server
  const showSegmentedBarChart = !!dashboardServerItem;

  return (
    <>
      {/* Single Organization total storage */}
      {showTotalStorage && (
        <TotalStorage
          serverItems={selectedServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
        />
      )}
      {/* Multiple OS */}
      {showAllocationByOS && (
        <AllocationByOS
          operatingSystemItems={selectedOperatingSystemItems}
          serverItems={selectedServerItems}
          loading={!isReadyOperatingSystemItems || !isReadyServerItems}
          onClick={async (operatingSystemItem) => {
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
          organizations={selectedOrganizations}
          serverItems={selectedServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
        />
      )}
      <StorageTrendsChart
        large={!!dashboardOrganization || !!dashboardOperatingSystemItem || !!dashboardServerItem}
      />
      {showAllocationByStorageVolume && (
        <AllocationByStorageVolume
          organizations={selectedOrganizations}
          serverItems={selectedServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
          onClick={async (organization) => {
            await updateDashboard({ organization });
          }}
        />
      )}
      {showAllocationTable && (
        <AllocationTable
          operatingSystemId={dashboardOperatingSystemItem?.id}
          serverItems={selectedServerItems}
          loading={!isReadyServerItems || !isReadyOperatingSystemItems}
          onClick={async (serverItem) => {
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
