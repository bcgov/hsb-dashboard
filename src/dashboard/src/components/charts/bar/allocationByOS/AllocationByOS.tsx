'use client';

import { useOperatingSystemItems } from '@/hooks/data';
import { useDashboard } from '@/store';
import Link from 'next/link';
import { BarRow, SmallBarChart } from '../smallBar';
import defaultData from './defaultData';
import { groupByOS } from './groupByOs';

export const AllocationByOS: React.FC = () => {
  const { serverItems } = useDashboard();
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
            key={os.label?.toString()}
            label={<Link href="">{os.label}</Link>}
            capacity={os.capacity}
            used={os.used}
            available={os.available}
          />
        ));
      }}
    </SmallBarChart>
  );
};
