'use client';

import { useOperatingSystemItems, useServerItems } from '@/hooks/data';
import Link from 'next/link';
import { BarRow, SmallBarChart } from '../smallBar';
import defaultData from './defaultData';
import { groupByOS } from './utils';

export const AllocationByOS: React.FC = () => {
  const { serverItems } = useServerItems();
  const { operatingSystemItems } = useOperatingSystemItems();

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
