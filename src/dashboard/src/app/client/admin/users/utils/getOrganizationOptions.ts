import { IOption } from '@/components';
import { IOrganizationModel } from '@/hooks/api/interfaces/auth';

export const getOrganizationOptions = (organizations: IOrganizationModel[]) => {
  return organizations
    .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
    .map<IOption<IOrganizationModel>>((t) => ({
      label: t.name,
      value: t.id,
      data: t,
    }));
};
