import { useDashboard } from '@/store';
import React from 'react';

export const useDashboardServerItems = () => {
  const serverItems = useDashboard((state) => state.serverItems);
  const setServerItems = useDashboard((state) => state.setServerItems);

  return React.useMemo(
    () => ({
      serverItems,
      setServerItems,
    }),
    [serverItems, setServerItems],
  );
};
