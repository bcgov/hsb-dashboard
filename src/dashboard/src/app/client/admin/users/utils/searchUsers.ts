import { IUserModel, RoleName } from '@/hooks';
import { IOrganizationModel, ITenantModel } from '@/hooks/api/interfaces/auth';
import { searchUsers as filter } from '@/utils';

export const searchUsers = (
  users: IUserModel[],
  search?: string,
  tenant?: ITenantModel,
  organization?: IOrganizationModel,
) => {
  return filter(users, search).filter((user) => {
    if (tenant && !user.tenants?.some((t) => t.id === tenant.id)) return false;
    if (organization && !user.organizations?.some((o) => o.id === organization.id)) return false;
    return !user.groups?.some(
      (group) => group.name === RoleName.HSB || group.name === RoleName.SystemAdmin,
    );
  });
};
