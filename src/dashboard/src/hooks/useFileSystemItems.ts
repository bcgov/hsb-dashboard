import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IFileSystemItemModel, useApiFileSystemItems, useAuth } from '.';

export const useFileSystemItems = () => {
  const { status } = useAuth();
  const { findFileSystemItems } = useApiFileSystemItems();
  const fileSystemItems = useApp((state) => state.fileSystemItems);
  const setFileSystemItems = useApp((state) => state.setFileSystemItems);

  React.useEffect(() => {
    // Get an array of fileSystemItems.
    if (status === 'authenticated' && !fileSystemItems.length) {
      findFileSystemItems().then(async (res) => {
        const fileSystemItems: IFileSystemItemModel[] = await res.json();
        setFileSystemItems(fileSystemItems);
      });
    }
  }, [findFileSystemItems, setFileSystemItems, status, fileSystemItems.length]);

  const options = React.useMemo(
    () =>
      fileSystemItems.map<IOption<IFileSystemItemModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [fileSystemItems],
  );

  return { fileSystemItems, options };
};
