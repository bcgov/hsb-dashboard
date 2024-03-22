'use client';

import { FilterDropdown } from '@/components';
import {
  IOperatingSystemItemListModel,
  IOrganizationListModel,
  IServerItemListModel,
  useAuth,
} from '@/hooks';
import {
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredServerItems,
  useFilteredTenants,
} from '@/hooks/filter';
import {
  useOperatingSystemItems,
  useOrganizations,
  useServerItems,
  useTenants,
} from '@/hooks/lists';
import { useFilteredStore } from '@/store';
import React from 'react';

export interface IFilteredTenantsProps {}

export const FilteredTenants = ({}: IFilteredTenantsProps) => {
  const { isHSB } = useAuth();
  const { isReady: tenantsReady, tenants } = useTenants();
  const { organizations } = useOrganizations();
  const { operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady, serverItems } = useServerItems();

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);
  const setLoading = useFilteredStore((state) => state.setLoading);

  const filteredTenants = useFilteredStore((state) => state.tenants);
  const setFilteredTenants = useFilteredStore((state) => state.setTenants);
  const { options: filteredTenantOptions } = useFilteredTenants();

  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const { findOrganizations } = useFilteredOrganizations();

  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { findServerItems } = useFilteredServerItems({
    useSimple: true,
  });

  const enableTenants = isHSB || tenants.length > 0;

  React.useEffect(() => {
    if (!filteredTenants.length && !!tenants.length) setFilteredTenants(tenants);
    if (tenants.length === 1) setValues((values) => ({ ...values, tenant: tenants[0] }));
  }, [filteredTenants.length, setFilteredTenants, setValues, tenants]);

  return filteredTenantOptions.length > 1 ? (
    <FilterDropdown
      label="Tenant"
      options={filteredTenantOptions}
      placeholder="Select tenant"
      value={values.tenant?.id ?? ''}
      disabled={!tenantsReady || !enableTenants || !serverItemsReady}
      loading={!tenantsReady}
      onChange={async (value) => {
        const tenant = tenants.find((t) => t.id == value);
        setLoading(true);
        setValues((state) => ({ ...state, tenant }));

        if (tenant) {
          let filteredOrganizations: IOrganizationListModel[];
          if (organizations.length) {
            filteredOrganizations = organizations.filter((org) =>
              org.tenants?.some((t) => t.id === tenant.id),
            );
            setFilteredOrganizations(filteredOrganizations);
          } else {
            filteredOrganizations = await findOrganizations({ tenantId: tenant.id });
          }
          const organization =
            filteredOrganizations.length === 1 ? filteredOrganizations[0] : undefined;

          // We filter server items before operating system items because the relationship belongs to the server item.
          let filteredServerItems: IServerItemListModel[];
          if (serverItems.length) {
            filteredServerItems = serverItems.filter((server) => server.tenantId === tenant.id);
            setFilteredServerItems(filteredServerItems);
          } else {
            filteredServerItems = await findServerItems({
              installStatus: 1,
              tenantId: tenant.id,
              organizationId: organization?.id,
            });
          }
          const serverItem = filteredServerItems?.length === 1 ? filteredServerItems[0] : undefined;

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
              tenantId: tenant.id,
              organizationId: organization?.id,
            });
          }
          const operatingSystemItem =
            filteredOperatingSystemItems.length === 1 ? filteredOperatingSystemItems[0] : undefined;

          setValues((state) => ({ tenant, organization, operatingSystemItem, serverItem }));
        } else {
          setFilteredOrganizations(organizations);
          setFilteredOperatingSystemItems(operatingSystemItems);
          setFilteredServerItems(serverItems);
        }
        setLoading(false);
      }}
    />
  ) : null;
};
