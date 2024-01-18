'use client';

import { AllocationTable, Col } from '@/components';
import { useSecureRoute } from '@/hooks';
import { useFilteredOrganizations, useFilteredServerItems } from '@/hooks/filter';

export default function Page() {
  useSecureRoute((state) => state.isClient, '/');

  const { organizations } = useFilteredOrganizations();
  const { serverItems } = useFilteredServerItems();

  return (
    <Col>
      <h1>All Servers</h1>
      <AllocationTable operatingSystem={''} serverItems={serverItems} />
    </Col>
  );
}
