import {
  AllOrganizations,
  AllocationByOS,
  AllocationByStorageVolume,
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
      <AllocationByOS />
    </>
  );
}
