import {
  AllOrgDonutChart,
  AllocationByStorageVolume,
  DonutChart,
  SmallBarChart,
  StorageTrendsChart,
} from '@/components/charts';

export default function Page() {
  return (
    <>
      <AllOrgDonutChart />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      <DonutChart />
      <SmallBarChart title="Allocation by OS" />
    </>
  );
}
