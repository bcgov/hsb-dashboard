import { IOrganizationModel } from '@/hooks';

export const searchOrganizations = (organizations: IOrganizationModel[], search?: string) => {
  const value = search?.toLowerCase();
  return value
    ? organizations.filter(
        (r) =>
          r.name.toLowerCase().includes(value) ||
          r.code.toLowerCase().includes(value) ||
          r.tenants?.some(
            (t) => t.name.toLowerCase().includes(value) || t.code.toLowerCase().includes(value),
          ),
      )
    : organizations;
};
