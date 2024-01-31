import { IServerHistoryItemFilter, IServerHistoryItemModel, useApiServerItems } from '@/hooks';
import { useStorageTrendsStore } from '@/store';
import React from 'react';

export const useServerHistoryItems = () => {
  const { history } = useApiServerItems();
  const serverHistoryItemsReady = useStorageTrendsStore((state) => state.serverHistoryItemsReady);
  const setServerHistoryItemsReady = useStorageTrendsStore(
    (state) => state.setServerHistoryItemsReady,
  );
  const serverHistoryItems = useStorageTrendsStore((state) => state.serverHistoryItems);
  const setServerHistoryItems = useStorageTrendsStore((state) => state.setServerHistoryItems);

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
