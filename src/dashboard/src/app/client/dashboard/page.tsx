import {
  AllOrganizations,
  AllocationByStorageVolume,
  SmallBarChart,
  StorageTrendsChart,
  TotalStorage,
} from '@/components/charts';

export default function Page() {
  return (
    <>
      <AllOrganizations />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      <TotalStorage />
      <SmallBarChart title="Allocation by OS" />
    </>
  );
}
