import { IOption } from '@/components';
import { useApp } from '@/store';
import React from 'react';
import { IServerItemModel, useApiServerItems, useAuth } from '.';

export const useServerItems = () => {
  const { status } = useAuth();
  const { findServerItems } = useApiServerItems();
  const serverItems = useApp((state) => state.serverItems);
  const setServerItems = useApp((state) => state.setServerItems);

  React.useEffect(() => {
    // Get an array of serverItems.
    if (status === 'authenticated' && !serverItems.length) {
      findServerItems().then(async (res) => {
        const serverItems: IServerItemModel[] = await res.json();
        setServerItems(serverItems);
      });
    }
  }, [findServerItems, setServerItems, status, serverItems.length]);

  const options = React.useMemo(
    () =>
      serverItems.map<IOption<IServerItemModel>>((t) => ({
        label: t.name,
        value: t.id,
        data: t,
      })),
    [serverItems],
  );

  return { serverItems, options };
};
