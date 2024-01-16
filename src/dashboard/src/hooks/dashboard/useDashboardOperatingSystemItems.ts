import { useDashboard } from '@/store';
import React from 'react';

export const useDashboardOperatingSystemItems = () => {
  const operatingSystemItems = useDashboard((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useDashboard((state) => state.setOperatingSystemItems);

  return React.useMemo(
    () => ({
      operatingSystemItems,
      setOperatingSystemItems,
    }),
    [operatingSystemItems, setOperatingSystemItems],
  );
};
