'use client';

import { Breadcrumbs } from '@/components/breadcrumbs';
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
import { IOperatingSystemItemListModel, IServerItemListModel, useApiServerItems } from '@/hooks';
import {
  useDashboardOperatingSystemItems,
  useDashboardOrganizations,
  useDashboardServerItems,
} from '@/hooks/dashboard';
import {
  useFilteredFileSystemItems,
  useFilteredOperatingSystemItems,
  useFilteredServerItems,
} from '@/hooks/filter';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/lists';
import { useDashboardStore, useFilteredStore } from '@/store';
import React from 'react';
import { toast } from 'react-toastify';
import { useDashboardFilter } from '.';

/**
 * Dashboard component displays different charts depending on what data has been stored in the dashboard state.
 * The Filter component updates the dashboard state when the Update button is clicked.
 * @returns Component
 */
export const Dashboard = () => {
  const { download, downloadHistory } = useApiServerItems();
  const { isReady: isReadyTenants, tenants } = useTenants({ init: true });
  const { isReady: isReadyOrganizations, organizations } = useOrganizations({
    init: true,
    includeTenants: true,
  });

  const { isReady: isReadyOperatingSystemItems, operatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );

  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();
  const { isReady: isReadyServerItems, serverItems } = useServerItems({
    init: true,
  });
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { findServerItems } = useFilteredServerItems({
    useSimple: true,
  });

  const dashboardTenant = useDashboardStore((state) => state.tenant);
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

  const [init, setInit] = React.useState(true);

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
  const showAllocationByStorageVolume =
    !dashboardOrganization && !dashboardOperatingSystemItem && !dashboardServerItem;
  // All servers with OS
  const showAllocationTable = !!dashboardOperatingSystemItem && !dashboardServerItem;
  // Show each drive over time for server
  const showSegmentedBarChart = !!dashboardServerItem;

  React.useEffect(() => {
    // When no filter is selected use all values available.
    if (
      isReadyTenants &&
      isReadyOrganizations &&
      isReadyOperatingSystemItems &&
      isReadyServerItems
    ) {
      if (
        !values.tenant &&
        !values.organization &&
        !values.operatingSystemItem &&
        !values.serverItem
      ) {
        updateDashboard({ reset: true });
      } else if (init) {
        updateDashboard({
          tenant: values.tenant,
          organization: values.organization,
          operatingSystemItem: values.operatingSystemItem,
          serverItem: values.serverItem,
          applyFilter: true,
        });
      }
      setInit(false);
    }
  }, [
    init,
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

  const handleExport = React.useCallback(
    async (filter: {
      search?: string;
      tenantId?: number;
      organizationId?: number;
      organizationName?: string;
      operatingSystemItemId?: number;
      serviceNowKey?: string;
      startDate?: string;
      endDate?: string;
    }) => {
      const toastLoading = toast.loading('Generating Excel document...');
      try {
        await download({
          search: filter.search,
          tenantId: filter.tenantId,
          organizationId: filter.organizationId,
          organizationName: filter.organizationName,
          operatingSystemItemId: filter.operatingSystemItemId,
          serviceNowKey: filter.serviceNowKey,
          startDate: filter.startDate,
          endDate: filter.endDate,
        });
        toast.dismiss(toastLoading);
        toast.success('Excel document has been downloaded successfully.');
      } catch (ex) {
        toast.dismiss(toastLoading);

        const error = ex as Error;
        toast.error('Failed to download data. ' + error.message);
        console.error(error);
      }
    },
    [download],
  );

  return (
    <>
      <Breadcrumbs multipleOrganizations={organizations.length > 1} />
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
            if (operatingSystemItem) {
              let filteredServerItems: IServerItemListModel[];
              if (serverItems.length) {
                filteredServerItems = serverItems.filter(
                  (server) =>
                    (values.tenant ? server.tenantId === values.tenant.id : true) &&
                    (values.organization
                      ? server.organizationId === values.organization.id
                      : true) &&
                    server.operatingSystemItemId === operatingSystemItem.id,
                );
                setFilteredServerItems(filteredServerItems ?? []);
              } else {
                filteredServerItems = await findServerItems({
                  installStatus: 1,
                  tenantId: values.tenant?.id,
                  organizationId: values.organization?.id,
                  operatingSystemItemId: operatingSystemItem.id,
                });
              }

              setValues((state) => ({ operatingSystemItem }));
              await updateDashboard({
                tenant: values.tenant,
                organization: values.organization,
                operatingSystemItem,
                serverItems: filteredServerItems,
                applyFilter: true,
              });
            } else {
              if (serverItems.length) {
                const filteredServerItems = serverItems.filter(
                  (server) =>
                    (values.tenant ? server.tenantId === values.tenant.id : true) &&
                    (values.organization ? server.organizationId === values.organization.id : true),
                );
                setFilteredServerItems(filteredServerItems ?? []);
              } else {
                await findServerItems({
                  installStatus: 1,
                  tenantId: values.tenant?.id,
                  organizationId: values.organization?.id,
                });
              }

              setValues((state) => ({}));
              await updateDashboard({});
            }
          }}
          showExport
          onExport={() => {
            handleExport({
              tenantId: dashboardTenant?.id,
              organizationId: dashboardOrganization?.id,
            });
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
          showExport
          onExport={() => {
            handleExport({ tenantId: dashboardTenant?.id });
          }}
        />
      )}
      <StorageTrendsChart
        large={!!dashboardOrganization || !!dashboardOperatingSystemItem || !!dashboardServerItem}
        serverItems={dashboardServerItem ? [dashboardServerItem] : dashboardServerItems}
        showExport
        onExport={(startDate, endDate) => {
          handleExport({
            tenantId: dashboardTenant?.id,
            organizationId: dashboardOrganization?.id,
            operatingSystemItemId: dashboardOperatingSystemItem?.id,
            serviceNowKey: dashboardServerItem?.serviceNowKey,
            startDate: startDate,
            endDate: endDate,
          });
        }}
      />
      {showAllocationByStorageVolume && (
        <AllocationByStorageVolume
          organizations={dashboardOrganizations}
          serverItems={dashboardServerItems}
          loading={!isReadyOrganizations || !isReadyServerItems}
          showExport
          onClick={async (organization) => {
            if (organization) {
              let filteredServerItems: IServerItemListModel[];
              if (serverItems.length) {
                filteredServerItems = serverItems.filter(
                  (server) =>
                    (values.tenant ? server.tenantId === values.tenant.id : true) &&
                    server.organizationId === organization.id &&
                    (values.operatingSystemItem
                      ? server.operatingSystemItemId === values.operatingSystemItem.id
                      : true),
                );
                setFilteredServerItems(filteredServerItems);
              } else {
                filteredServerItems = await findServerItems({
                  installStatus: 1,
                  tenantId: values.tenant?.id,
                  organizationId: organization?.id,
                });
              }
              const serverItem =
                filteredServerItems?.length === 1 ? filteredServerItems[0] : undefined;

              let filteredOperatingSystemItems: IOperatingSystemItemListModel[];
              if (operatingSystemItems.length) {
                // Only return operating system items that match available server items.
                const osIds = filteredServerItems
                  .map((server) => server.operatingSystemItemId)
                  .filter((id, index, array) => !!id && array.indexOf(id) === index);
                filteredOperatingSystemItems = operatingSystemItems.filter((os) =>
                  osIds.some((id) => id === os.id),
                );
                setFilteredOperatingSystemItems(filteredOperatingSystemItems);
              } else {
                filteredOperatingSystemItems = await findOperatingSystemItems({
                  tenantId: values.tenant?.id,
                  organizationId: organization.id,
                });
              }
              const operatingSystemItem =
                filteredOperatingSystemItems.length === 1
                  ? filteredOperatingSystemItems[0]
                  : undefined;

              setValues((state) => ({ organization, operatingSystemItem, serverItem }));
              await updateDashboard({
                tenant: values.tenant,
                organization,
                serverItems: filteredServerItems,
                operatingSystemItems: filteredOperatingSystemItems,
                applyFilter: true,
              });
            } else {
              setFilteredOperatingSystemItems(operatingSystemItems);
              if (serverItems.length) {
                const filteredServerItems = serverItems.filter(
                  (server) =>
                    (values.tenant ? server.tenantId === values.tenant.id : true) &&
                    (values.operatingSystemItem
                      ? server.operatingSystemItemId === values.operatingSystemItem.id
                      : true),
                );
                setFilteredServerItems(filteredServerItems ?? []);
              } else {
                await findServerItems({
                  installStatus: 1,
                  tenantId: values.tenant?.id,
                  operatingSystemItemId: values.operatingSystemItem?.id,
                });
              }

              setValues((state) => ({}));
              await updateDashboard({});
            }
          }}
          onExport={(search) => {
            handleExport({
              tenantId: dashboardTenant?.id,
              organizationName: search ? search : undefined,
            });
          }}
        />
      )}
      {showAllocationTable && (
        <AllocationTable
          operatingSystemId={dashboardOperatingSystemItem?.id}
          serverItems={dashboardServerItems}
          loading={!isReadyServerItems || !isReadyOperatingSystemItems}
          onClick={async (serverItem) => {
            const tenant = tenants.find((tenant) => tenant.id === serverItem?.tenantId);
            const organization = organizations.find(
              (organization) => organization.id === serverItem?.organizationId,
            );
            const operatingSystemItem = operatingSystemItems.find(
              (operatingSystemItem) => operatingSystemItem.id === serverItem?.operatingSystemItemId,
            );
            setValues((state) => ({ serverItem, tenant, organization, operatingSystemItem }));
            await updateDashboard({ tenant, organization, operatingSystemItem, serverItem });
          }}
          showExport
          onExport={(search) => {
            handleExport({
              tenantId: dashboardTenant?.id,
              organizationId: dashboardOrganization?.id,
              operatingSystemItemId: dashboardOperatingSystemItem?.id,
              search: search ? search : undefined,
            });
          }}
        />
      )}
      {showSegmentedBarChart && (
        <SegmentedBarChart serverItem={dashboardServerItem} loading={!isReadyServerItems} />
      )}
    </>
  );
};
