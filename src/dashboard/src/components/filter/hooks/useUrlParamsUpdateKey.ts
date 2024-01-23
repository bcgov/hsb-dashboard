import { useSearchParams } from 'next/navigation';
import React from 'react';

/**
 * Generates a lock and an initial key to control when the dashboard should be updated based on the URL parameters.
 * Without this it will update the dashboard multiple times.
 * @returns Two keys that must match before it will update the dashboard.
 */
export const useUrlParamsUpdateKey = () => {
  const params = useSearchParams();

  const currentParams = React.useMemo(
    () => new URLSearchParams(Array.from(params.entries())),
    [params],
  );

  const readyKey = React.useRef(0);
  var lockKey = 0;

  // Extract URL query parameters and initialize state.
  const tenantId = currentParams.get('tenant');
  const organizationId = currentParams.get('organization');
  const operatingSystemItemId = currentParams.get('operatingSystemItem');
  const serverItemKey = currentParams.get('serverItem');

  // Generate a lock that matches the provided URL parameters.
  if (tenantId) lockKey = lockKey | 1;
  if (organizationId) lockKey = lockKey | 2;
  if (operatingSystemItemId) lockKey = lockKey | 4;
  if (serverItemKey) lockKey = lockKey | 8;

  return { readyKey, lockKey };
};
