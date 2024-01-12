import { useDashboard } from '@/store';
import React from 'react';
import {
  IFileSystemHistoryItemFilter,
  IFileSystemHistoryItemModel,
  useApiFileSystemItems,
} from '..';

export const useDashboardFileSystemHistoryItems = () => {
  const { history } = useApiFileSystemItems();
  const fileSystemHistoryItemsReady = useDashboard((state) => state.fileSystemHistoryItemsReady);
  const setFileSystemHistoryItemsReady = useDashboard(
    (state) => state.setFileSystemHistoryItemsReady,
  );
  const fileSystemHistoryItems = useDashboard((state) => state.fileSystemHistoryItems);
  const setFilteredFileSystemHistoryItems = useDashboard(
    (state) => state.setFileSystemHistoryItems,
  );

  const fetch = React.useCallback(
    async (filter: IFileSystemHistoryItemFilter) => {
      try {
        setFileSystemHistoryItemsReady(false);
        const res = await history(filter);
        const fileSystemHistoryItems: IFileSystemHistoryItemModel[] = await res.json();
        setFilteredFileSystemHistoryItems(fileSystemHistoryItems);
        return fileSystemHistoryItems;
      } catch (error) {
        throw error;
      } finally {
        setFileSystemHistoryItemsReady(true);
      }
    },
    [history, setFilteredFileSystemHistoryItems, setFileSystemHistoryItemsReady],
  );

  return React.useMemo(
    () => ({
      isReady: fileSystemHistoryItemsReady,
      findFileSystemHistoryItems: fetch,
      fileSystemHistoryItems,
    }),
    [fileSystemHistoryItemsReady, fetch, fileSystemHistoryItems],
  );
};
