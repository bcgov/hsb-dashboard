'use client';

import styles from './Organizations.module.scss';

import { Button, Checkbox, Info, Overlay, Sheet, Spinner, Table, Text } from '@/components';
import { IOrganizationModel, useAuth } from '@/hooks';
import { useApiOrganizations } from '@/hooks/api/admin';
import { useOrganizations } from '@/hooks/data';
import { useAppStore } from '@/store';
import { searchOrganizations } from '@/utils';
import { redirect } from 'next/navigation';
import React from 'react';
import { IOrganizationForm } from './IOrganizationForm';
import { LoadingAnimation } from '@/components/charts/loadingAnimation';

export default function Page() {
  const state = useAuth();
  const { isReady: isReadyOrganizations, organizations } = useOrganizations({
    init: true,
    includeTenants: true,
  });
  const { update: updateOrganization } = useApiOrganizations();
  const setOrganizations = useAppStore((state) => state.setOrganizations);

  const [loading, setLoading] = React.useState(true);
  const [formOrganizations, setFormOrganizations] = React.useState<IOrganizationForm[]>([]);
  const [filteredOrganizations, setFilteredOrganizations] = React.useState<number[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);

  React.useEffect(() => {
    setLoading(!isReadyOrganizations);
  }, [isReadyOrganizations]);

  React.useEffect(() => {
    setFormOrganizations((state) =>
      organizations.map((organization) => {
        const value = state.find((s) => s.id === organization.id);
        if (value) ({ ...organization, isDirty: value.isDirty });
        return organization;
      }),
    );
    setFilteredOrganizations(searchOrganizations(organizations, filter).map((o) => o.id));
    // Only apply the 'filter' when the button is pressed.
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [organizations]);

  const handleSearch = React.useCallback(() => {
    setFilteredOrganizations(searchOrganizations(formOrganizations, filter).map((o) => o.id));
  }, [filter, formOrganizations]);

  const handleUpdate = React.useCallback(async () => {
    const update = formOrganizations.map(async (organization) => {
      if (organization.isDirty) {
        try {
          setIsSubmitting(true);
          const res = await updateOrganization(organization);
          const result: IOrganizationModel = await res.json();
          return { ...result, isDirty: false };
        } catch (error) {
          console.error(error);
        } finally {
          setIsSubmitting(false);
        }
      }
      return organization;
    });
    const results = await Promise.all(update);
    const updatedOrganizations = formOrganizations.map((organization) => {
      return results.find((u) => u.id === organization.id) ?? organization;
    });
    setFormOrganizations(updatedOrganizations);
    setOrganizations(updatedOrganizations);
  }, [formOrganizations, setOrganizations, updateOrganization]);

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <LoadingAnimation />;
  if (!state.isSystemAdmin) redirect('/');

  return (
    <div className={styles.panelContainer}>
      <Sheet>
        <div className={styles.container}>
          {loading && (
            <LoadingAnimation />
          )}
          <div className={styles.section}>
            <Info>
              Find an organization by name and/or associated tenant. Click checkbox to enable or
              disable organization on dashboard.
            </Info>
            <div className={styles.search}>
              <Text
                name="search"
                placeholder="Search"
                iconType="search"
                onChange={(e) => setFilter(e.target.value)}
                onKeyDown={(e) => {
                  if (e.code === 'Enter') handleSearch();
                }}
              />
              <Button variant="secondary" onClick={() => handleSearch()}>
                Search
              </Button>
            </div>
          </div>
          <div>
            <Table
              data={formOrganizations.filter((o) =>
                filteredOrganizations.some((fo) => fo === o.id),
              )}
              header={
                <>
                  <div>Organization Name</div>
                  <div>Associated Tenants</div>
                  <div>Dashboard Enabled</div>
                </>
              }
            >
              {({ data }) => {
                return (
                  <>
                    <div>{data.name}</div>
                    <div>{data.tenants?.map((tenant) => tenant.name).join(', ')}</div>
                    <div className={styles.checkbox}>
                      <Checkbox
                        checked={data.isEnabled}
                        onChange={(e) => {
                          setFormOrganizations((formOrganizations) =>
                            formOrganizations.map((r) =>
                              r.id === data.id
                                ? { ...data, isEnabled: e.target.checked, isDirty: true }
                                : r,
                            ),
                          );
                        }}
                      />
                    </div>
                  </>
                );
              }}
            </Table>
          </div>
          <div className={styles.footer}>
            <Button
              disabled={isSubmitting || !formOrganizations.some((r) => r.isDirty)}
              onClick={() => handleUpdate()}
            >
              Save
            </Button>
          </div>
        </div>
      </Sheet>
    </div>
  );
}
