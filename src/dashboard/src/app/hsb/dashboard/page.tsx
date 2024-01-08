import {
  AllOrganizations,
  AllocationByStorageVolume,
  SmallBarChart,
  StorageTrendsChart,
  TotalStorage,
  AllocationTable
} from '@/components/charts';

export default function Page() {
  return (
    <>
      <AllOrganizations />
      <StorageTrendsChart large={false} />
      <AllocationByStorageVolume />
      <TotalStorage />
      <SmallBarChart title="Allocation by OS" />
      <AllocationTable />
    </>
  );
}
