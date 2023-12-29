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
  groups: string[];
  roles: string[];
  preferences: { [key: string]: any };
  version?: number;
}
