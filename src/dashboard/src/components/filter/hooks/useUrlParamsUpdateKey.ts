import { useSearchParams } from 'next/navigation';
import React from 'react';

/**
 * Generates a lock and an initial key to control when the dashboard should be updated based on the URL parameters.
 * Without this it will update the dashboard multiple times.
 * @returns Two keys that must match before it will update the dashboard.
 */
export const useUrlParamsUpdateKey = () => {
  const params = useSearchParams();

  const readyKey = React.useRef(0);
  const lockKey = React.useRef(0);

  React.useEffect(() => {
    const currentParams = new URLSearchParams(Array.from(params.entries()));
    // Extract URL query parameters and initialize state.
    const tenantId = currentParams.get('tenant');
    const organizationId = currentParams.get('organization');
    const operatingSystemItemId = currentParams.get('operatingSystemItem');
    const serverItemKey = currentParams.get('serverItem');

    // Generate a lock that matches the provided URL parameters.
    if (tenantId) lockKey.current = lockKey.current | 1;
    if (organizationId) lockKey.current = lockKey.current | 2;
    if (operatingSystemItemId) lockKey.current = lockKey.current | 4;
    if (serverItemKey) lockKey.current = lockKey.current | 8;
  }, [params]);

  return { readyKey, lockKey: lockKey.current };
};
