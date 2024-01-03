import {
  AllOrgDonutChart,
  AllocationByStorageVolume,
  StorageTrendsChart,
  DonutChart,
  SmallBarChart,
} from '@/components/charts';

export default function Page() {
  return (
    <>
      {/* HSB Dashboard */}
      <AllOrgDonutChart />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      <DonutChart />
      <SmallBarChart title="Allocation by OS" />
      {/* <LineChart large='true' /> */}
    </>
  );
}
