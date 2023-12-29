import { AllOrgDonutChart } from '@/components/allOrgDonutChart';
import { LineChart } from '@/components/lineChart';
import { OrganizationsChart } from '@/components/allOrgBarChart';

export default function Page() {
  return (
    <>
      {/* HSB Dashboard */}
      <AllOrgDonutChart />
      <LineChart />
      <OrganizationsChart />
      <LineChart large='true' />
    </>
  );
}
