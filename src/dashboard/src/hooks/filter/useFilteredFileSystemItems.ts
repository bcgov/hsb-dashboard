import { useFilteredStore } from '@/store';
import React from 'react';
import { IFileSystemItemFilter, IFileSystemItemModel, useApiFileSystemItems } from '..';

export const useFilteredFileSystemItems = () => {
  const { find } = useApiFileSystemItems();
  const isLoading = useFilteredStore((state) => state.loadingFileSystemItems);
  const setIsLoading = useFilteredStore((state) => state.setLoadingFileSystemItems);
  const fileSystemItems = useFilteredStore((state) => state.fileSystemItems);
  const setFileSystemItems = useFilteredStore((state) => state.setFileSystemItems);

  const fetch = React.useCallback(
    async (filter: IFileSystemItemFilter) => {
      try {
        setIsLoading(true);
        const res = await find(filter);
        const fileSystemItems: IFileSystemItemModel[] = await res.json();
        setFileSystemItems(fileSystemItems);
        return fileSystemItems;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [find, setFileSystemItems, setIsLoading],
  );

  return React.useMemo(
    () => ({ isLoading, findFileSystemItems: fetch, fileSystemItems }),
    [isLoading, fileSystemItems, fetch],
  );
};
