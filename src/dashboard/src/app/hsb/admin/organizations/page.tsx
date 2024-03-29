'use client';

import styles from './Organizations.module.scss';

import { AdminLoadingAnimation, Button, Checkbox, Info, Sheet, Table, Text } from '@/components';
import { LoadingAnimation } from '@/components/loadingAnimation';
import { IOrganizationModel, useAuth } from '@/hooks';
import { useAdminOrganizations } from '@/hooks/admin';
import { useApiOrganizations } from '@/hooks/api/admin';
import { useAdminStore, useNavigateStore } from '@/store';
import { searchOrganizations } from '@/utils';
import { redirect } from 'next/navigation';
import React from 'react';
import { toast } from 'react-toastify';
import { IOrganizationForm } from './IOrganizationForm';

export default function Page() {
  const state = useAuth();
  const { isReady: isReadyOrganizations, organizations } = useAdminOrganizations({
    init: true,
    includeTenants: true,
  });
  const { update: updateOrganization } = useApiOrganizations();
  const setOrganizations = useAdminStore((state) => state.setOrganizations);
  const setEnableNavigate = useNavigateStore((state) => state.setEnableNavigate);

  const [loading, setLoading] = React.useState(true);
  const [formOrganizations, setFormOrganizations] = React.useState<IOrganizationForm[]>([]);
  const [filteredOrganizations, setFilteredOrganizations] = React.useState<number[]>([]);
  const [filter, setFilter] = React.useState('');
  const [isSubmitting, setIsSubmitting] = React.useState(false);

  const isDirty = formOrganizations.some((o) => o.isDirty);

  React.useEffect(() => {
    setLoading(!isReadyOrganizations);
  }, [isReadyOrganizations]);

  React.useEffect(() => {
    setEnableNavigate(!isDirty);
  }, [isDirty, setEnableNavigate]);

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
    setIsSubmitting(true);
    const update = formOrganizations.map(async (organization) => {
      if (organization.isDirty) {
        try {
          const res = await updateOrganization(organization);
          const result: IOrganizationModel = await res.json();
          return { ...result, isDirty: false };
        } catch (ex) {
          const error = ex as Error;
          toast.error(error.message);
          console.error(error);
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
    setIsSubmitting(false);
  }, [formOrganizations, setOrganizations, updateOrganization]);

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <AdminLoadingAnimation />;
  if (!state.isSystemAdmin) redirect('/');

  return (
    <div className={styles.panelContainer}>
      <Sheet>
        <div className={styles.container}>
          {(loading || isSubmitting) && <LoadingAnimation />}
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
