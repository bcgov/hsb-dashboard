import {
  AllOrgDonutChart,
  AllocationByStorageVolume,
  LineChart,
} from '@/components/charts';

export default function Page() {
  return (
    <>
      {/* HSB Dashboard */}
      <AllOrgDonutChart />
      <LineChart />
      <AllocationByStorageVolume />
      {/* <LineChart large='true' /> */}
    </>
  );
}
