import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { IServerItemFilter, IServerItemModel, useApiServerItems } from '..';

export const useFilteredServerItems = () => {
  const { find } = useApiServerItems();
  const serverItems = useFiltered((state) => state.serverItems);
  const setFilteredServerItems = useFiltered((state) => state.setServerItems);

  const fetch = React.useCallback(
    async (filter: IServerItemFilter) => {
      const res = await find(filter);
      const serverItems: IServerItemModel[] = await res.json();
      setFilteredServerItems(serverItems);
      return serverItems;
    },
    [find, setFilteredServerItems],
  );

  const options = React.useMemo(
    () =>
      serverItems.map<IOption<IServerItemModel>>((t) => ({
        label: t.name ? `${t.name}` : '[NO NAME PROVIDED]',
        value: t.serviceNowKey,
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
