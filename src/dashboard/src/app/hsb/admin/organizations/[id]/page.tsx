'use client';

import styles from '../Organizations.module.scss';
import { useAuth } from '@/hooks';
import { redirect, usePathname } from 'next/navigation';
import { Button, Checkbox, Overlay, Sheet, Spinner, Text, Toggle, Select } from '@/components';
import React from 'react';
interface IPageProps {
  params: { id: string; searchParams: any };
}

export default function Page({ params }: IPageProps) {
  const [loading, setLoading] = React.useState(true);
  const path = usePathname();
  const isUpdate = !path.includes('/0');

  const state = useAuth();

  // Only allow System Admin role to view this page.
  if (state.status === 'loading') return <div>Loading...</div>;
  if (!state.isSystemAdmin) redirect('/');

  return (
    <div className={styles.panelContainer}>
      <Sheet>
        <div className={styles.container}>
          {/* {loading && (
        <Overlay>
          <Spinner />
        </Overlay>
      )} */}
          {isUpdate && <div className={styles.selectUpdate}>
            <p>Select Organization</p>
            <div>
              <Select 
                options={[]}
                placeholder="Select Organization" 
              />
              <Button>Proceed</Button>
            </div>
          </div>}
          <section>
            <div>
              <p>Enabled (visible on dashboard)</p>
              <Toggle />
            </div>
            <div className={styles.description}>
              <p>Organization description</p>
              <textarea placeholder="Organization description" />
            </div>
          </section>
          <section>
            <div>
              <p>Allow access to the following tenants</p>
              <div className={styles.tableContainer}>
                <div className={styles.search}>
                  <Text name="search" placeholder="Search" iconType="search" />
                  <Button variant="secondary">Search</Button>
                </div>
                <div className={styles.rows}>
                  <div className={styles.row}>
                    <Checkbox />
                    <p>Tenant</p>
                  </div>
                </div>
              </div>
            </div>
            <div>
              <p>Include the following organizations</p>
              <div className={styles.tableContainer}>
                <div className={styles.search}>
                  <Text name="search" placeholder="Search" iconType="search" />
                  <Button variant="secondary">Search</Button>
                </div>
                <div className={styles.rows}>
                  <div className={styles.row}>
                    <Checkbox />
                    <p>Organization</p>
                  </div>
                </div>
              </div>
            </div>
          </section>
          <div className={styles.footer}>
            {!isUpdate && 
              <>
                <Button variant="secondary" iconPath="/images/trash-icon.svg">Discard</Button>
                <Button>Add New Organization</Button>
              </>
            }
            {isUpdate && 
              <>
                <Button variant="secondary" iconPath="/images/trash-icon.svg">Delete</Button>
                <Button>Update Organization</Button>
              </>
            }
          </div>
        </div>
      </Sheet>
    </div>
  );
}
