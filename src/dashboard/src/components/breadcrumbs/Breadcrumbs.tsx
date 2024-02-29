import styles from './Breadcrumbs.module.scss';
import { useDashboardStore } from '@/store';

interface BreadcrumbsProps {
    multipleOrganizations: boolean;
}

export const Breadcrumbs: React.FC<BreadcrumbsProps> = ({ multipleOrganizations }) => {
  const {
    organization,
    operatingSystemItem,
    serverItem,
  } = useDashboardStore(state => ({
    organization: state.organization,
    operatingSystemItem: state.operatingSystemItem,
    serverItem: state.serverItem,
  }));

  return (
    <div className={styles.breadcrumbs}>
       {!!multipleOrganizations && (
        <div>
            <p>All Organizations</p>
        </div>
       )}
      {!!organization && (
        <div>
          <p title={organization.name}>{organization.name}</p>
        </div>
      )}
      {!!operatingSystemItem && (
        <div>
          <p title={operatingSystemItem.name}>{operatingSystemItem.name}</p>
        </div>
      )}
      {!!serverItem && (
        <div>
          <p title={serverItem.name}>{serverItem.name}</p>
        </div>
      )}
    </div>
  );
};
