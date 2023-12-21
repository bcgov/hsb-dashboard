import { IAuditableModel } from '.';

export interface ICommonModel<T extends string | number> extends IAuditableModel {
  id: T;
  name: string;
  description: string;
  isEnabled: boolean;
}
