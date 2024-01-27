import { IOption } from '@/components';
import { ITenantModel } from '@/hooks/api/interfaces/auth';

export const getTenantOptions = (tenants: ITenantModel[]) => {
  return tenants
    .sort((a, b) => (a.name < b.name ? -1 : a.name > b.name ? 1 : 0))
    .map<IOption<ITenantModel>>((t) => ({
      label: t.name,
      value: t.id,
      data: t,
    }));
};
