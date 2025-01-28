CREATE MATERIALIZED VIEW "mvServerHistoryItemsByMonth" AS
SELECT DISTINCT ON ("ServiceNowKey", DATE_TRUNC('month', "CreatedOn"))
    "Id",
    "TenantId",
    "OrganizationId",
    "OperatingSystemItemId",
    "ServiceNowKey",
    "HistoryKey",
    '{}'::jsonb AS "RawData",
    '{}'::jsonb AS "RawDataCI",
    "ClassName",
    "Name",
    "Category",
    "Subcategory",
    "DnsDomain",
    "Platform",
    "IPAddress",
    "FQDN",
    "DiskSpace",
    "Capacity",
    "AvailableSpace",
    "CreatedOn",
    "CreatedBy",
    "UpdatedOn",
    "UpdatedBy",
    "Version",
    "InstallStatus"
FROM "ServerHistoryItem"
WHERE "CreatedOn" >= NOW() - INTERVAL '1 year'
ORDER BY "ServiceNowKey", DATE_TRUNC('month', "CreatedOn"), "CreatedOn" DESC;
