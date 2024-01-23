import { IOption } from '@/components';
import { useFiltered } from '@/store';
import React from 'react';
import { IServerItemFilter, IServerItemModel, useApiServerItems } from '..';

export interface IFilteredServerItemsProps {
  useSimple?: boolean;
}

export const useFilteredServerItems = (
  { useSimple = false }: IFilteredServerItemsProps | undefined = { useSimple: false },
) => {
  const { find, findSimple } = useApiServerItems();
  const serverItems = useFiltered((state) => state.serverItems);
  const setFilteredServerItems = useFiltered((state) => state.setServerItems);

  const fetch = React.useCallback(
    async (filter: IServerItemFilter) => {
      const res = useSimple ? await findSimple(filter) : await find(filter);
      const serverItems: IServerItemModel[] = await res.json();
      setFilteredServerItems(serverItems);
      return serverItems;
    },
    [find, findSimple, setFilteredServerItems, useSimple],
  );

  const options = React.useMemo(
    () =>
      serverItems
        .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
        .map<IOption<IServerItemModel>>((t) => ({
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
