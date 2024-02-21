export interface IFileSystemHistoryItemFilter {
  name?: string;
  serviceNowKey?: string;
  tenantId?: number;
  organizationId?: number;
  operatingSystemItemId?: number;
  serverItemServiceNowKey?: string;
  category?: string;
  subCategory?: string;
  className?: string;
  startDate?: string;
  endDate?: string;
  installStatus?: number;
  notInstallStatus?: number;
}
