'use client';

import { AllocationByStorageVolume, Col } from '@/components';
import { useSecureRoute } from '@/hooks';
import { useFilteredOrganizations, useFilteredServerItems } from '@/hooks/filter';

export default function Page() {
  useSecureRoute((state) => state.isClient, '/');

  const { organizations } = useFilteredOrganizations();
  const { serverItems } = useFilteredServerItems();

  return (
    <Col>
      <h1></h1>All Servers
      <AllocationByStorageVolume organizations={organizations} serverItems={serverItems} />
    </Col>
  );
}
