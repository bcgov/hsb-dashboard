'use client';

import { Select } from '@/components';
import { IServerItemModel, useAuth } from '@/hooks';
import { useOperatingSystemItems, useOrganizations, useServerItems } from '@/hooks/data';
import {
  useFilteredOperatingSystemItems,
  useFilteredOrganizations,
  useFilteredServerItems,
} from '@/hooks/filter';
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
    if (organizations.length) setFilteredOrganizations(organizations);
    if (organizations.length === 1)
      setValues((values) => ({ ...values, organization: organizations[0] }));
  }, [setFilteredOrganizations, organizations, setValues]);

  return (
    <Select
      label="Organization"
      variant="filter"
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
          const operatingSystems = await findOperatingSystemItems({
            tenantId: values.tenant?.id,
            organizationId: organization.id,
          });
          const operatingSystemItem =
            operatingSystems.length === 1 ? operatingSystems[0] : undefined;

          let filteredServerItems: IServerItemModel[];
          if (serverItems.length) {
            filteredServerItems = serverItems.filter(
              (server) =>
                (values.tenant ? server.tenantId === values.tenant.id : true) &&
                server.organizationId === organization.id &&
                (values.operatingSystemItem
                  ? server.operatingSystemItemId === values.operatingSystemItem.id
                  : true),
            );
          } else {
            filteredServerItems = await findServerItems({
              tenantId: values.tenant?.id,
              organizationId: organization?.id,
              operatingSystemItemId: operatingSystemItem?.id,
            });
          }
          const serverItem = filteredServerItems?.length === 1 ? filteredServerItems[0] : undefined;

          setFilteredServerItems(filteredServerItems ?? []);
          setValues((state) => ({ ...state, organization, operatingSystemItem, serverItem }));
        } else {
          setFilteredOperatingSystemItems(operatingSystemItems);
          let filteredServerItems: IServerItemModel[];
          if (serverItems.length) {
            filteredServerItems = serverItems.filter(
              (server) =>
                (values.tenant ? server.tenantId === values.tenant.id : true) &&
                (values.operatingSystemItem
                  ? server.operatingSystemItemId === values.operatingSystemItem.id
                  : true),
            );
          } else {
            filteredServerItems = await findServerItems({
              tenantId: values.tenant?.id,
              operatingSystemItemId: values.operatingSystemItem?.id,
            });
          }
          setFilteredServerItems(filteredServerItems ?? []);
        }
        setLoading(false);
      }}
    />
  );
};
