import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { IServerItemFilter, IServerItemModel, useApiServerItems } from '..';

export const useFilteredServerItems = () => {
  const { findServerItems } = useApiServerItems();
  const serverItems = useFiltered((state) => state.serverItems);
  const setServerItems = useFiltered((state) => state.setServerItems);

  const fetch = React.useCallback(
    async (filter: IServerItemFilter) => {
      const res = await findServerItems(filter);
      const serverItems: IServerItemModel[] = await res.json();
      setServerItems(serverItems);
      return serverItems;
    },
    [findServerItems, setServerItems],
  );

  const options = React.useMemo(
    () =>
      serverItems.map<IOption<IServerItemModel>>((t) => ({
        label: t.name ? `${t.name}` : '[NO NAME PROVIDED]',
        value: t.id,
        data: t,
      })),
    [serverItems],
  );

  return React.useMemo(
    () => ({
      findServerItems: fetch,
      options,
      serverItems,
    }),
    [serverItems, fetch, options],
  );
};
