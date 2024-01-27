import { IOrganizationModel, ITenantModel } from '.';

export interface IUserInfoModel {
  id: number;
  key?: string;
  username?: string;
  email?: string;
  displayName?: string;
  firstName?: string;
  lastName?: string;
  lastLoginOn?: string;
  isEnabled: boolean;
  note: string;
  tenants: ITenantModel[];
  organizations: IOrganizationModel[];
  groups: string[];
  roles: string[];
  preferences: { [key: string]: any };
  version?: number;
}
