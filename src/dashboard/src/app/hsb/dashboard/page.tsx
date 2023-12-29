import { Col, Row } from '@/components';
import {
  AllOrgDonutChart,
  AllocationByStorageVolumeChart,
  UnusedSpaceChart,
} from '@/components/charts';
import styles from './Page.module.scss';

export default function Page() {
  return (
    <Col className={styles.page}>
      <Row>
        <AllOrgDonutChart />
        <UnusedSpaceChart />
      </Row>
      <AllocationByStorageVolumeChart />
    </Col>
  );
}
