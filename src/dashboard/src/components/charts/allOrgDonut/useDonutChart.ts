import { useFiltered } from '@/store';
import React from 'react';
import { IStats } from './IStats';
import { defaultData } from './defaultData';
import { updateData } from './updateData';

export const useDonutChart = () => {
  const serverItem = useFiltered((state) => state.serverItem);
  const serverItems = useFiltered((state) => state.serverItems);

  const [data, setData] = React.useState<IStats>(defaultData);

  React.useEffect(() => {
    if (serverItem) {
      setData((data) => updateData(serverItem.capacity, serverItem.availableSpace, data));
    } else if (serverItems.length) {
      const space = serverItems.map((si) => si.capacity!).reduce((a, b) => (a ?? 0) + (b ?? 0));
      const available = serverItems
        .map((si) => si.availableSpace!)
        .reduce((a, b) => (a ?? 0) + (b ?? 0));
      setData((data) => updateData(space, available, data));
    } else {
      setData(defaultData);
    }
  }, [serverItems, serverItem]);

  return data;
};
