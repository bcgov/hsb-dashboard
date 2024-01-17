'use client';

import { IOperatingSystemItemModel, IServerItemModel } from '@/hooks';
import Link from 'next/link';
import { BarRow, SmallBarChart } from '../smallBar';
import defaultData from './defaultData';
import { groupByOS } from './utils';

export interface IAllocationByOSProps {
  serverItems: IServerItemModel[];
  operatingSystemItems: IOperatingSystemItemModel[];
  loading?: boolean;
}

export const AllocationByOS = ({
  serverItems,
  operatingSystemItems,
  loading,
}: IAllocationByOSProps) => {
  return (
    <SmallBarChart
      title="Allocation by OS"
      data={{ ...defaultData, datasets: groupByOS(serverItems, operatingSystemItems) }}
      exportDisabled={true}
      onExport={() => {}}
    >
      {(data) => {
        return data.datasets.map((os) => (
          <BarRow
            key={os.key}
            label={<Link href="">{os.label}</Link>}
            capacity={os.capacity}
            available={os.available}
          />
        ));
      }}
    </SmallBarChart>
  );
};
