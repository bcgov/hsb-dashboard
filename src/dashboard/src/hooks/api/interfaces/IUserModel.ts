import { IAuditableModel } from '.';
import { IGroupModel } from './IGroupModel';
import { ITenantModel } from './ITenantModel';

export interface IUserModel extends IAuditableModel {
  id: number;
  username: string;
  email: string;
  emailVerified: boolean;
  key: string;
  displayName: string;
  firstName: string;
  middleName: string;
  lastName: string;
  phone: string;
  isEnabled: boolean;
  failedLogins: number;
  lastLoginOn?: string;
  note: string;
  preferences: any;
  groups?: IGroupModel[];
  tenants?: ITenantModel[];
}
