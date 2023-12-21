'use client';

import { AllOrgDonutChart } from '@/components/allOrgDonutChart';
import { Sheet } from '@/components';

export default function Page() {
  return (
    <div className="dashboardContainer">
      HSB Dashboard
      <AllOrgDonutChart />
    </div>
  );
}
