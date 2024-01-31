import { useDashboardStore } from '@/store';
import React from 'react';

export const useDashboardServerItems = () => {
  const serverItem = useDashboardStore((state) => state.serverItem);
  const serverItems = useDashboardStore((state) => state.serverItems);
  const setServerItems = useDashboardStore((state) => state.setServerItems);

  return React.useMemo(
    () => ({
      serverItem,
      serverItems,
      setServerItems,
    }),
    [serverItem, serverItems, setServerItems],
  );
};
