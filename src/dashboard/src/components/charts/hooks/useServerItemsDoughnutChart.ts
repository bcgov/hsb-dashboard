import { useDashboard } from '@/store';
import React from 'react';
import { IDoughnutStats } from '../doughnut/allOrganizations/IDoughnutStats';
import { defaultData } from '../doughnut/allOrganizations/defaultData';
import { generateDoughnutChart } from './generateDoughnutChart';

export const useServerItemsDoughnutChart = () => {
  const serverItems = useDashboard((state) => state.serverItems);

  const [data, setData] = React.useState<IDoughnutStats>(defaultData);

  React.useEffect(() => {
    if (serverItems.length) {
      const space = serverItems.map((si) => si.capacity!).reduce((a, b) => (a ?? 0) + (b ?? 0));
      const available = serverItems
        .map((si) => si.availableSpace!)
        .reduce((a, b) => (a ?? 0) + (b ?? 0));
      setData((data) => generateDoughnutChart(space, available, data));
    } else {
      setData(defaultData);
    }
  }, [serverItems]);

  return data;
};
