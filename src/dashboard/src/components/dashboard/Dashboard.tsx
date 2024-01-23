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
import { useDashboard } from '@/store';
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
  const { organizations: dashboardOrganizations } = useDashboardOrganizations();
  const { operatingSystemItems: dashboardOperatingSystemItems } =
    useDashboardOperatingSystemItems();
  const { serverItems: dashboardServerItems } = useDashboardServerItems();
  const dateRange = useDashboard((state) => state.dateRange);
  const { fileSystemItems } = useFilteredFileSystemItems();

  const updateDashboard = useDashboardFilter();

  const selectedOrganizations = dashboardOrganizations.length
    ? dashboardOrganizations
    : organizations;
  const selectedOperatingSystemItems = dashboardOperatingSystemItems.length
    ? dashboardOperatingSystemItems
    : operatingSystemItems;
  const selectedServerItems = dashboardServerItems.length ? dashboardServerItems : serverItems;

  const showTotalStorage =
    selectedServerItems.length === 1 ||
    (selectedOrganizations.length === 1 && selectedOperatingSystemItems.length > 1);
  const showAllocationByOS =
    selectedOrganizations.length === 1 &&
    selectedOperatingSystemItems.length > 1 &&
    selectedServerItems.length !== 1;
  const showAllocationByVolume = selectedServerItems.length === 1;
  const showAllOrganizations =
    selectedOrganizations.length > 1 &&
    selectedOperatingSystemItems.length > 1 &&
    selectedServerItems.length > 1;
  const showAllocationByStorageVolume =
    selectedOrganizations.length > 1 &&
    selectedOperatingSystemItems.length > 1 &&
    selectedServerItems.length > 1;
  const showAllocationTable =
    selectedOperatingSystemItems.length === 1 && selectedServerItems.length > 1;
  const showSegmentedBarChart = selectedServerItems.length === 1;

  return (
    <>
      {/* Single Organization total storage */}
      {showTotalStorage && <TotalStorage serverItems={selectedServerItems} />}
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
        <AllocationByVolume fileSystemItems={fileSystemItems} loading={false} />
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
        large={
          selectedOrganizations.length === 1 ||
          selectedOperatingSystemItems.length === 1 ||
          selectedServerItems.length === 1
        }
        dateRange={dateRange}
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
          operatingSystem={selectedServerItems[0].className}
          serverItems={selectedServerItems}
          loading={!isReadyServerItems || !isReadyOperatingSystemItems}
          onClick={async (serverItem) => {
            console.debug(serverItem);
            await updateDashboard({ serverItem });
          }}
        />
      )}
      {showSegmentedBarChart && (
        <SegmentedBarChart
          serverItem={selectedServerItems[0]}
          loading={!isReadyServerItems}
          dateRange={dateRange}
        />
      )}
    </>
  );
};
