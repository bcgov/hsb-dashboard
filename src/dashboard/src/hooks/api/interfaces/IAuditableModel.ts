export interface IAuditableModel {
  createdOn: string;
  createdBy: string;
  updatedOn: string;
  updatedBy: string;
  version: number;
}
