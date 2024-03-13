import { Button, Checkbox, Text } from '@/components';
import { IUserForm } from '@/components/admin';
import styles from './Users.module.scss';

export interface IAddNewUserRowProps {
  index: number;
  values: IUserForm;
  errors?: { [Key in keyof IUserForm]?: string };
  onChange?: (values: IUserForm) => void;
  onEditGroups?: (values: IUserForm) => void;
  onEditOrganizations?: (values: IUserForm) => void;
  onEditTenants?: (values: IUserForm) => void;
  onRemove?: () => void;
}

/**
 * Provides a table row to display a form to add a new user.
 * @param param0 Component properties.
 * @returns Component.
 */
export const AddNewUserRow = ({
  index,
  values,
  errors,
  onChange,
  onEditGroups,
  onEditOrganizations,
  onEditTenants,
  onRemove,
}: IAddNewUserRowProps) => {
  return (
    <div className={styles.addNewRow}>
      <Button variant="secondary" onClick={onRemove}></Button>
      <div>
        <Text
          name={`${index}.username`}
          placeholder="Username"
          value={values.username}
          required={true}
          onChange={(e) => onChange?.({ ...values, username: e.target.value, isDirty: true })}
          error={errors?.username}
        />
      </div>
      <div>
        <Text
          name={`${index}.email`}
          placeholder="Email"
          value={values.email}
          required={true}
          onChange={(e) => onChange?.({ ...values, email: e.target.value, isDirty: true })}
          error={errors?.email}
        />
      </div>
      <div>
        <Text
          name={`${index}.displayName`}
          placeholder="Name"
          value={values.displayName}
          required={true}
          onChange={(e) => onChange?.({ ...values, displayName: e.target.value, isDirty: true })}
          error={errors?.displayName}
        />
      </div>
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
    </div>
  );
};
