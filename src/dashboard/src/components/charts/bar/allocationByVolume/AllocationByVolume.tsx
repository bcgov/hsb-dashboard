'use client';

import { IFileSystemItemModel } from '@/hooks';
import Link from 'next/link';
import { BarRow, SmallBarChart } from '../smallBar';
import { IBarChartRowData } from '../smallBar/IBarChartRowData';
import defaultData from './defaultData';

export interface IAllocationByVolumeProps {
  fileSystemItems: IFileSystemItemModel[];
  loading?: boolean;
}

export const AllocationByVolume = ({ fileSystemItems, loading }: IAllocationByVolumeProps) => {
  return (
    <SmallBarChart
      title="Drive Space"
      data={{
        ...defaultData,
        datasets: fileSystemItems.map<IBarChartRowData>((fsi) => ({
          key: fsi.name,
          label: fsi.name,
          capacity: fsi.capacity,
          available: fsi.availableSpace,
        })),
      }}
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
