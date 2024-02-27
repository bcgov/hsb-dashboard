import { Button, Checkbox } from '@/components';
import { IUserForm } from '@/components/admin';
import styles from './Users.module.scss';

export interface IEditUserRowProps {
  index: number;
  values: IUserForm;
  onChange?: (values: IUserForm) => void;
  onEditGroups?: (values: IUserForm) => void;
  onEditOrganizations?: (values: IUserForm) => void;
  onEditTenants?: (values: IUserForm) => void;
}

export const EditUserRow = ({
  index,
  values,
  onChange,
  onEditGroups,
  onEditOrganizations,
  onEditTenants,
}: IEditUserRowProps) => {
  return (
    <>
      <div>{values.username}</div>
      <div>{values.email}</div>
      <div>{values.displayName}</div>
      <div className={styles.checkbox}>
        <Checkbox
          name={`${index}.isEnabled`}
          checked={values.isEnabled}
          onChange={(e) => onChange?.({ ...values, isEnabled: e.target.checked, isDirty: true })}
        />
      </div>
      <div className={styles.selectRow}>
        <div>
          <p>
            <span>Roles: </span>
            {values.groups?.map((org) => org.name).join(', ')}
          </p>
          <Button variant="secondary" onClick={() => onEditGroups?.(values)}>
            Edit
          </Button>
        </div>
        <div>
          <p>
            <span>Organizations: </span>
            {values.organizations?.map((org) => org.name).join(', ')}
          </p>
          <Button variant="secondary" onClick={() => onEditOrganizations?.(values)}>
            Edit
          </Button>
        </div>
        <div>
          <p>
            <span>Tenant: </span>
            {values.tenants?.map((org) => org.name).join(', ')}
          </p>
          <Button variant="secondary" onClick={() => onEditTenants?.(values)}>
            Edit
          </Button>
        </div>
      </div>
    </>
  );
};
