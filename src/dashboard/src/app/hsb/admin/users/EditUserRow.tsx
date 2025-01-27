import { Button, Checkbox, Text } from '@/components';
import { IUserForm, UserDialogVariant } from '@/components/admin';
import styles from './Users.module.scss';
import React, { useEffect } from 'react';

export interface IEditUserRowProps {
  index: number;
  values: IUserForm;
  errors?: { [Key in keyof IUserForm]?: string };
  setErrors?: React.Dispatch<
    React.SetStateAction<{ [key: string]: { [K in keyof IUserForm]?: string } }>
  >;
  onChange?: (values: IUserForm) => void;
  handleEditClick?: (user: IUserForm, variant: UserDialogVariant) => void;
}

export const EditUserRow = ({
  index,
  values,
  errors,
  setErrors,
  onChange,
  handleEditClick,
}: IEditUserRowProps) => {
  const [isEditingTextFields, setIsEditingTextFields] = React.useState(false);

  const originalValues = React.useRef<IUserForm>(values);

  const cancelEditing = () => {
    setIsEditingTextFields(false);
    setErrors?.((errors) => ({ ...errors, [values.key]: {} }));
    onChange?.(originalValues.current);
  };

  useEffect(() => {
    // If the user was saved, update the "original" values. We do this by comparing the version. Also, stop editing.
    if (values.version > originalValues.current.version) {
      originalValues.current = values;
      setIsEditingTextFields(false);
    }
  }, [values]);

  return (
    <>
      {!isEditingTextFields && (
        <>
          <div className={styles.editButton}>
            <Button variant="secondary" onClick={() => setIsEditingTextFields(true)}>
              Edit
            </Button>
          </div>
          <div>{values.username}</div>
          <div>{values.email}</div>
          <div>{values.displayName}</div>
        </>
      )}
      {isEditingTextFields && (
        <>
          <div>
            <Button variant="secondary" onClick={cancelEditing}>
              Cancel
            </Button>
          </div>
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
              placeholder="Display name"
              value={values.displayName}
              required={true}
              onChange={(e) =>
                onChange?.({ ...values, displayName: e.target.value, isDirty: true })
              }
              error={errors?.displayName}
            />
          </div>
        </>
      )}
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
          <Button variant="secondary" onClick={() => handleEditClick?.(values, 'group')}>
            Edit
          </Button>
        </div>
        <div>
          <p>
            <span>Organizations: </span>
            {values.organizations?.map((org) => org.name).join(', ')}
          </p>
          <Button variant="secondary" onClick={() => handleEditClick?.(values, 'organization')}>
            Edit
          </Button>
        </div>
        <div>
          <p>
            <span>Tenant: </span>
            {values.tenants?.map((org) => org.name).join(', ')}
          </p>
          <Button variant="secondary" onClick={() => handleEditClick?.(values, 'tenant')}>
            Edit
          </Button>
        </div>
      </div>
    </>
  );
};
