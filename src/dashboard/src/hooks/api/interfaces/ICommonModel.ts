import { IAuditModel } from '.';

export interface ICommonModel<T extends string | number> extends IAuditModel {
  id: T;
  name: string;
  description: string;
  isEnabled: boolean;
}
