export interface IServerItemFilter {
  search?: string;
  name?: string;
  serviceNowKey?: string;
  operatingSystemItemId?: number;
  organizationId?: number;
  organizationName?: string;
  tenantId?: number;
  startDate?: string;
  endDate?: string;
  installStatus?: number;
  notInstallStatus?: number;
  updatedBeforeDate?: string;
}
