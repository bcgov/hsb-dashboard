import { IServerItemModel } from '@/hooks';
import { defaultData } from '../defaultData';
import { generateDoughnutChart } from './generateDoughnutChart';

export const generateAllOrganizationsDoughnutChart = (serverItems: IServerItemModel[]) => {
  if (serverItems.length) {
    const space = serverItems.map((si) => si.capacity!).reduce((a, b) => (a ?? 0) + (b ?? 0));
    const available = serverItems
      .map((si) => si.availableSpace!)
      .reduce((a, b) => (a ?? 0) + (b ?? 0));
    return generateDoughnutChart(space, available, defaultData);
  }
  return defaultData;
};
