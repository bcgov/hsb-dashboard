'use client';

import { FilterDropdown } from '@/components';
import { IOperatingSystemItemListModel, IServerItemListModel, useAuth } from '@/hooks';
import {
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredServerItems,
} from '@/hooks/filter';
import { useOperatingSystemItems, useOrganizations, useServerItems } from '@/hooks/lists';
import { useFilteredStore } from '@/store';
import React from 'react';

export interface IFilteredOrganizationsProps {}

export const FilteredOrganizations = ({}: IFilteredOrganizationsProps) => {
  const { isHSB } = useAuth();
  const { isReady: organizationsReady, organizations } = useOrganizations();
  const { operatingSystemItems } = useOperatingSystemItems();
  const { isReady: serverItemsReady, serverItems } = useServerItems();

  const values = useFilteredStore((state) => state.values);
  const setValues = useFilteredStore((state) => state.setValues);
  const setLoading = useFilteredStore((state) => state.setLoading);

  const filteredOrganizations = useFilteredStore((state) => state.organizations);
  const setFilteredOrganizations = useFilteredStore((state) => state.setOrganizations);
  const { options: filteredOrganizationOptions } = useFilteredOrganizations();

  const setFilteredOperatingSystemItems = useFilteredStore(
    (state) => state.setOperatingSystemItems,
  );
  const { findOperatingSystemItems } = useFilteredOperatingSystemItems();

  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);
  const { findServerItems } = useFilteredServerItems({
    useSimple: true,
  });

  const enableOrganizations = isHSB || organizations.length > 0;

  React.useEffect(() => {
    if (!filteredOrganizations.length && !!organizations.length)
      setFilteredOrganizations(organizations);
    if (organizations.length === 1)
      setValues((values) => ({ ...values, organization: organizations[0] }));
  }, [setFilteredOrganizations, organizations, setValues, filteredOrganizations.length]);

  return filteredOrganizationOptions.length > 1 ? (
    <FilterDropdown
      label="Organization"
      options={filteredOrganizationOptions}
      placeholder="Select organization"
      value={values.organization?.id ?? ''}
      disabled={!organizationsReady || !enableOrganizations || !serverItemsReady}
      loading={!organizationsReady}
      onChange={async (value) => {
        const organization = organizations.find((o) => o.id == value);
        setLoading(true);
        setValues((state) => ({ ...state, organization }));

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
              tenantId: values.tenant?.id,
              organizationId: organization.id,
            });
          }
          const operatingSystemItem =
            filteredOperatingSystemItems.length === 1 ? filteredOperatingSystemItems[0] : undefined;

          setValues((state) => ({ ...state, organization, operatingSystemItem, serverItem }));
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
        }
        setLoading(false);
      }}
    />
  ) : null;
};
