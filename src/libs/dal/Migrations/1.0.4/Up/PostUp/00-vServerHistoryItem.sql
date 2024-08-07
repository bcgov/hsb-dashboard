CREATE OR REPLACE VIEW "vServerHistoryItem" AS
SELECT
    "Id"
    , "TenantId"
    , "OrganizationId"
    , "OperatingSystemItemId"
    , "ServiceNowKey"
    , "HistoryKey"
    , "ClassName"
    , "Name"
    , "Category"
    , "Subcategory"
    , "DnsDomain"
    , "Platform"
    , "IPAddress"
    , "FQDN"
    , "DiskSpace"
    , "Capacity"
    , "AvailableSpace"
    , "CreatedOn"
    , "CreatedBy"
    , "UpdatedOn"
    , "UpdatedBy"
    , "Version"
    , "InstallStatus"
FROM public."ServerHistoryItem";
