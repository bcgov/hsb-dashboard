import {
  AllOrgDonutChart,
  AllocationByStorageVolume,
  StorageTrendsChart,
  DonutChart,
} from '@/components/charts';

export default function Page() {
  return (
    <>
      {/* HSB Dashboard */}
      <AllOrgDonutChart />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      <DonutChart />
      {/* <LineChart large='true' /> */}
    </>
  );
}
