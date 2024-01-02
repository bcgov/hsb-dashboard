import {
  AllOrgDonutChart,
  AllocationByStorageVolume,
  StorageTrendsChart,
} from '@/components/charts';

export default function Page() {
  return (
    <>
      {/* HSB Dashboard */}
      <AllOrgDonutChart />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      {/* <LineChart large='true' /> */}
    </>
  );
}
