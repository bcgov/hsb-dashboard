import { useDashboard } from '@/store';
import React from 'react';
import { IServerHistoryItemFilter, IServerHistoryItemModel, useApiServerItems } from '..';

export const useDashboardServerHistoryItems = () => {
  const { history } = useApiServerItems();
  const serverHistoryItemsReady = useDashboard((state) => state.serverHistoryItemsReady);
  const setServerHistoryItemsReady = useDashboard((state) => state.setServerHistoryItemsReady);
  const serverHistoryItems = useDashboard((state) => state.serverHistoryItems);
  const setServerHistoryItems = useDashboard((state) => state.setServerHistoryItems);

  const fetch = React.useCallback(
    async (filter: IServerHistoryItemFilter) => {
      try {
        setServerHistoryItemsReady(false);
        const res = await history(filter);
        const serverHistoryItems: IServerHistoryItemModel[] = await res.json();
        setServerHistoryItems(serverHistoryItems);
        return serverHistoryItems;
      } catch (error) {
        throw error;
      } finally {
        setServerHistoryItemsReady(true);
      }
    },
    [history, setServerHistoryItems, setServerHistoryItemsReady],
  );

  return React.useMemo(
    () => ({ isReady: serverHistoryItemsReady, findServerHistoryItems: fetch, serverHistoryItems }),
    [serverHistoryItemsReady, serverHistoryItems, fetch],
  );
};
