import { useDashboardStore } from '@/store';
import React from 'react';

export const useDashboardOperatingSystemItems = () => {
  const operatingSystemItem = useDashboardStore((state) => state.operatingSystemItem);
  const operatingSystemItems = useDashboardStore((state) => state.operatingSystemItems);
  const setOperatingSystemItems = useDashboardStore((state) => state.setOperatingSystemItems);

  return React.useMemo(
    () => ({ operatingSystemItem, operatingSystemItems, setOperatingSystemItems }),
    [operatingSystemItem, operatingSystemItems, setOperatingSystemItems],
  );
};
