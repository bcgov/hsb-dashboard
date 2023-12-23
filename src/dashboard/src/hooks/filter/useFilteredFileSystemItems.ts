import { useFiltered } from '@/store';
import React from 'react';
import { IFileSystemItemFilter, IFileSystemItemModel, useApiFileSystemItems } from '..';

export const useFilteredFileSystemItems = () => {
  const { findFileSystemItems } = useApiFileSystemItems();
  const fileSystemItems = useFiltered((state) => state.fileSystemItems);
  const setFileSystemItems = useFiltered((state) => state.setFileSystemItems);

  const fetch = React.useCallback(
    async (filter: IFileSystemItemFilter) => {
      const res = await findFileSystemItems(filter);
      const fileSystemItems: IFileSystemItemModel[] = await res.json();
      setFileSystemItems(fileSystemItems);
      return fileSystemItems;
    },
    [findFileSystemItems, setFileSystemItems],
  );

  return React.useMemo(
    () => ({
      findFileSystemItems: fetch,
      fileSystemItems,
    }),
    [fileSystemItems, fetch],
  );
};
