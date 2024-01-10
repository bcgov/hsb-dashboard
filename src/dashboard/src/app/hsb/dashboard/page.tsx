import {
  AllOrganizations,
  AllocationByOS,
  AllocationByStorageVolume,
  AllocationTable,
  StorageTrendsChart,
  TotalStorage,
  SmallBarChart,
  SegmentedBarChart
} from '@/components/charts';
import { OperatingSystems } from '@/components/charts/allocationTable/constants';

export default function Page() {
  return (
    <>
      <AllOrganizations />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      <TotalStorage />
      <AllocationByOS />
      <AllocationTable operatingSystem={OperatingSystems.Windows} />
      <SegmentedBarChart />
    </>
  );
}
