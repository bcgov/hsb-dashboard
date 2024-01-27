import { IOption } from '@/components';
import { IOrganizationModel } from '@/hooks';

export const getOrganizationOptions = (
  organizations: IOrganizationModel[],
  includeDisabled?: boolean,
) => {
  return organizations
    .filter((t) => (includeDisabled ? true : t.isEnabled))
    .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
    .map<IOption<IOrganizationModel>>((t) => ({
      label: t.name,
      value: t.id,
      data: t,
      disabled: t.isEnabled,
    }));
};
