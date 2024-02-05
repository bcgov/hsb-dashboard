import { Button, Checkbox, Dialog, IDialogProps } from '@/components';
import { Text } from '@/components/forms/text';
import { useGroups, useOrganizations, useTenants } from '@/hooks/data';
import React from 'react';
import { IUserForm } from '../';
import styles from './UserDialog.module.scss';

export type UserDialogVariant = 'organization' | 'tenant' | 'group';

export interface IUserDialogProps extends Omit<IDialogProps, 'onChange'> {
  variant?: UserDialogVariant;
  user?: IUserForm;
  onChange: (user: IUserForm) => void;
  onSave: (user?: IUserForm) => void;
}

export const UserDialog = React.forwardRef<HTMLDialogElement, IUserDialogProps>(function UserDialog(
  { variant, user: initUser, header, actions, onChange, onSave, ...rest },
  ref,
) {
  const { groups } = useGroups({ init: true });
  const { organizations } = useOrganizations({ init: true });
  const { tenants } = useTenants({ init: true });

  const [filteredGroups, setFilteredGroups] = React.useState(groups);
  const [filteredOrganizations, setFilteredOrganizations] = React.useState(organizations);
  const [filteredTenants, setFilteredTenants] = React.useState(tenants);
  const [form, setForm] = React.useState(initUser);

  React.useEffect(() => {
    setFilteredGroups(groups);
  }, [groups]);

  React.useEffect(() => {
    setFilteredOrganizations(organizations);
  }, [organizations]);

  React.useEffect(() => {
    setFilteredTenants(tenants);
  }, [tenants]);

  React.useEffect(() => {
    setForm(initUser);
  }, [initUser]);

  var title = React.useMemo(() => {
    if (variant === 'group') {
      return 'Editing roles';
    } else if (variant === 'organization') {
      return 'Allow access to the following organizations';
    } else if (variant === 'tenant') {
      return 'Allow access to the following tenants';
    }
    return '';
  }, [variant]);

  var options: React.ReactElement[] = React.useMemo(() => {
    if (variant === 'group') {
      return filteredGroups.map((group, index) => (
        <div key={group.id} className={styles.row}>
          <Checkbox
            id={`group-${group.id}`}
            name={`groups.${index}.id`}
            checked={form?.groups?.some((g) => g.id === group.id) === true}
            onChange={(e) => {
              if (form) {
                const user = {
                  ...form,
                  groups: e.target.checked
                    ? [...(form.groups ?? []), group]
                    : form.groups?.filter((g) => g.id !== group.id),
                };
                setForm(user);
                onChange?.(user);
              }
            }}
          />
          <p>
            <label htmlFor={`group-${group.id}`}>{group.name}</label>
          </p>
        </div>
      ));
    } else if (variant === 'organization') {
      return filteredOrganizations.map((organization, index) => (
        <div key={organization.id} className={styles.row}>
          <Checkbox
            id={`organization-${organization.id}`}
            name={`organizations.${index}.id`}
            checked={form?.organizations?.some((g) => g.id === organization.id) === true}
            onChange={(e) => {
              if (form) {
                const user = {
                  ...form,
                  organizations: e.target.checked
                    ? [...(form.organizations ?? []), organization]
                    : form.organizations?.filter((g) => g.id !== organization.id),
                };
                setForm(user);
                onChange?.(user);
              }
            }}
          />
          <p>
            <label htmlFor={`organization-${organization.id}`}>{organization.name}</label>
          </p>
        </div>
      ));
    } else if (variant === 'tenant') {
      return filteredTenants.map((tenant, index) => (
        <div key={tenant.id} className={styles.row}>
          <Checkbox
            id={`tenant-${tenant.id}`}
            name={`tenants.${index}.id`}
            checked={form?.tenants?.some((g) => g.id === tenant.id) === true}
            onChange={(e) => {
              if (form) {
                const user = {
                  ...form,
                  tenants: e.target.checked
                    ? [...(form.tenants ?? []), tenant]
                    : form.tenants?.filter((g) => g.id !== tenant.id),
                };
                setForm(user);
                onChange?.(user);
              }
            }}
          />
          <p>
            <label htmlFor={`tenant-${tenant.id}`}>{tenant.name}</label>
          </p>
        </div>
      ));
    }
    return [];
  }, [variant, filteredGroups, form, onChange, filteredOrganizations, filteredTenants]);

  const handleSearch = React.useCallback(
    (value: string) => {
      if (variant === 'group') {
        setFilteredGroups(
          groups.filter((group) => group.name.toLowerCase().includes(value.toLowerCase())),
        );
      } else if (variant === 'organization') {
        setFilteredOrganizations(
          organizations.filter((organization) =>
            organization.name.toLowerCase().includes(value.toLowerCase()),
          ),
        );
      } else if (variant === 'tenant') {
        setFilteredTenants(
          tenants.filter((tenant) => tenant.name.toLowerCase().includes(value.toLowerCase())),
        );
      }
    },
    [variant, groups, organizations, tenants],
  );

  return (
    <Dialog
      header={
        <p>
          {title} for {form?.username}
        </p>
      }
      actions={<Button onClick={() => onSave(form)}>Save</Button>}
      {...rest}
      ref={ref}
    >
      <div className={styles.popupSearch}>
        <Text
          name="search"
          placeholder="Search"
          iconType="search"
          onChange={(e) => handleSearch(e.target.value)}
        />
      </div>
      <div className={styles.popupRows}>{options}</div>
    </Dialog>
  );
});
