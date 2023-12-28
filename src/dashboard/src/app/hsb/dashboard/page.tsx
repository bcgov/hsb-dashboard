import { AllOrgDonutChart } from '@/components/allOrgDonutChart';
import { LineChart } from '@/components/lineChart';

export default function Page() {
  return (
    <>
      {/* HSB Dashboard */}
      <AllOrgDonutChart />
      <LineChart />
      <LineChart large='true' />
    </>
  );
}
