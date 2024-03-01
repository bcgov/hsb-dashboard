import { IOption } from '@/components';
import { useFilteredStore } from '@/store';
import React from 'react';
import { IServerItemFilter, IServerItemListModel, useApiServerItems } from '..';

export interface IFilteredServerItemsProps {}

export const useFilteredServerItems = ({}: IFilteredServerItemsProps) => {
  const { findList } = useApiServerItems();
  const serverItems = useFilteredStore((state) => state.serverItems);
  const setFilteredServerItems = useFilteredStore((state) => state.setServerItems);

  const [isLoading, setIsLoading] = React.useState(false);

  const fetch = React.useCallback(
    async (filter: IServerItemFilter) => {
      try {
        setIsLoading(true);
        const res = await findList(filter);
        const serverItems: IServerItemListModel[] = await res.json();
        setFilteredServerItems(serverItems);
        return serverItems;
      } catch (error) {
        throw error;
      } finally {
        setIsLoading(false);
      }
    },
    [findList, setFilteredServerItems],
  );

  const options = React.useMemo(
    () =>
      serverItems
        .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
        .map<IOption<IServerItemListModel>>((t) => ({
          label: t.name ? `${t.name}` : '[NO NAME PROVIDED]',
          value: t.serviceNowKey,
          data: t,
        })),
    [serverItems],
  );

  return React.useMemo(
    () => ({ isLoading, findServerItems: fetch, options, serverItems }),
    [isLoading, serverItems, fetch, options],
  );
};
