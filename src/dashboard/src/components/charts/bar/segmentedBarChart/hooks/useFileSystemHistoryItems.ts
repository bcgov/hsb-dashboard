import {
  IFileSystemHistoryItemFilter,
  IFileSystemHistoryItemModel,
  useApiFileSystemItems,
} from '@/hooks';
import { useStorageTrendsStore } from '@/store';
import React from 'react';

export const useFileSystemHistoryItems = () => {
  const { history } = useApiFileSystemItems();
  const fileSystemHistoryItemsReady = useStorageTrendsStore(
    (state) => state.fileSystemHistoryItemsReady,
  );
  const setFileSystemHistoryItemsReady = useStorageTrendsStore(
    (state) => state.setFileSystemHistoryItemsReady,
  );
  const fileSystemHistoryItems = useStorageTrendsStore((state) => state.fileSystemHistoryItems);
  const setFilteredFileSystemHistoryItems = useStorageTrendsStore(
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
