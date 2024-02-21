export interface IServerItemFilter {
  name?: string;
  serviceNowKey?: string;
  operatingSystemItemId?: number;
  organizationId?: number;
  tenantId?: number;
  startDate?: string;
  endDate?: string;
  installStatus?: number;
  notInstallStatus?: number;
  updatedBeforeDate?: string;
}
