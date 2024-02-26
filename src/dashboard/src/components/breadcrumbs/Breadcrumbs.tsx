import styles from './Breadcrumbs.module.scss';
import { useFilteredStore } from '@/store';

interface BreadcrumbsProps {
    multipleOrganizations: boolean;
}

export const Breadcrumbs: React.FC<BreadcrumbsProps> = ({ multipleOrganizations }) => {
  const values = useFilteredStore((state) => state.values);

  return (
    <div className={styles.breadcrumbs}>
       {!!multipleOrganizations && (
        <div>
            <p>All Organizations</p>
        </div>
       )}
      {!!values.organization && (
        <div>
          <p title={values.organization.name}>{values.organization.name}</p>
        </div>
      )}
      {!!values.operatingSystemItem && (
        <div>
          <p title={values.operatingSystemItem.name}>{values.operatingSystemItem.name}</p>
        </div>
      )}
      {!!values.serverItem && (
        <div>
          <p title={values.serverItem.name}>{values.serverItem.name}</p>
        </div>
      )}
    </div>
  );
};
