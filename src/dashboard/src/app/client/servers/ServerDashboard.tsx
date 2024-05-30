'use client';

import { AllocationTable, useDashboardFilter } from '@/components';
import { useApiServerItems } from '@/hooks';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/lists';
import { useFilteredStore } from '@/store';
import { useRouter } from 'next/navigation';
import { toast } from 'react-toastify';

export default function ServerDashboard() {
  const router = useRouter();
  const { tenants } = useTenants({ init: true });
  const { organizations } = useOrganizations({
    init: true,
    includeTenants: true,
  });
  const { operatingSystemItems } = useOperatingSystemItems({
    init: true,
  });
  const { isReady, serverItems } = useServerItems({ init: true });
  const setFilteredValues = useFilteredStore((state) => state.setValues);
  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { download } = useApiServerItems();

  const updateDashboard = useDashboardFilter();

  return (
    <AllocationTable
      margin={-75}
      serverItems={serverItems}
      loading={!isReady}
      onClick={async (serverItem) => {
        if (serverItem) {
          const tenant = tenants.find((tenant) => tenant.id === serverItem?.tenantId);
          const organization = organizations.find(
            (organization) => organization.id === serverItem?.organizationId,
          );
          const operatingSystemItem = operatingSystemItems.find(
            (operatingSystemItem) => operatingSystemItem.id === serverItem?.operatingSystemItemId,
          );
          const filteredOrganizations = tenant
            ? organizations.filter((o) => o.tenants?.some((t) => t.id === tenant?.id))
            : organizations;
          const filteredServerItems = serverItems.filter(
            (si) =>
              (!tenant || si.tenantId === tenant.id) &&
              (!organization || si.organizationId === organization.id) &&
              (!operatingSystemItem || si.operatingSystemItemId === operatingSystemItem.id),
          );
          setFilteredOrganizations(filteredOrganizations);
          setFilteredServerItems(filteredServerItems);
          setFilteredValues((state) => ({
            serverItem,
            tenant,
            organization,
            operatingSystemItem,
          }));
          await updateDashboard({
            tenant,
            tenants,
            organization,
            organizations: filteredOrganizations,
            operatingSystemItem,
            operatingSystemItems,
            serverItem,
            serverItems: filteredServerItems,
            fetchFileSystemItems: false,
          });
          router.push(`/client/dashboard?serverItem=${serverItem?.serviceNowKey}`);
        }
      }}
      showExport
      onExport={async (search) => {
        const toastLoading = toast.loading('Generating Excel document...');

        try {
          await download({
            search: search ? search : undefined,
          });

          toast.dismiss(toastLoading);
          toast.success('Excel document has been downloaded successfully.');
        } catch (ex) {
          toast.dismiss(toastLoading);

          const error = ex as Error;
          toast.error('Failed to download data. ' + error.message);
          console.error(error);
        }
      }}
    />
  );
}
