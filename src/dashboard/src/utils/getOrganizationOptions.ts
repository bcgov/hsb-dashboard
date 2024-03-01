import { IOption } from '@/components';
import { IOrganizationListModel, IOrganizationModel } from '@/hooks';

export const getOrganizationOptions = <T extends IOrganizationModel | IOrganizationListModel>(
  organizations: T[],
  includeDisabled?: boolean,
) => {
  return organizations
    .filter((t) => (includeDisabled ? true : t.isEnabled))
    .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
    .map<IOption<T>>((t) => ({
      label: t.name,
      value: t.id,
      data: t,
      disabled: t.isEnabled,
    }));
};
