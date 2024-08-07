DROP FUNCTION "FindFileSystemHistoryItemsByMonthForUser";

CREATE OR REPLACE FUNCTION "FindFileSystemHistoryItemsByMonthForUser"(
  "userId" INT
  , "startDate" TIMESTAMPTZ
  , "endDate" TIMESTAMPTZ DEFAULT NULL
  , "tenantId" INT DEFAULT NULL
  , "organizationId" INT DEFAULT NULL
  , "operatingSystemItemId" INT DEFAULT NULL
  , "serverServiceNowKey" VARCHAR(200) DEFAULT NULL
)
RETURNS SETOF public."FileSystemHistoryItem"
LANGUAGE plpgsql
AS $$
DECLARE
  end_date_default TIMESTAMPTZ;
  last_month_start TIMESTAMPTZ;
BEGIN
	end_date_default := (SELECT COALESCE($3, NOW()));
	last_month_start := (SELECT (DATE_TRUNC('month', end_date_default))::TIMESTAMPTZ);
  RETURN QUERY
  SELECT DISTINCT
    "Id"
    , "ServiceNowKey"
    , "RawData"
    , "RawDataCI"
    , "Name"
    , "Label"
    , "Category"
    , "Subcategory"
    , "StorageType"
    , "MediaType"
    , "VolumeId"
    , "ClassName"
    , "Capacity"
    , "DiskSpace"
    , "Size"
    , "SizeBytes"
    , "UsedSizeBytes"
    , "AvailableSpace"
    , "FreeSpace"
    , "FreeSpaceBytes"
    , "CreatedOn"
    , "CreatedBy"
    , "UpdatedOn"
    , "UpdatedBy"
    , "Version"
    , "ServerItemServiceNowKey"
    , "InstallStatus"
  FROM (
    SELECT fshi.*
      , ROW_NUMBER() OVER (PARTITION BY fshi."ServiceNowKey", EXTRACT(YEAR FROM fshi."CreatedOn"), EXTRACT(MONTH FROM fshi."CreatedOn") ORDER BY fshi."CreatedOn" DESC) AS "rn"
    FROM public."FileSystemHistoryItem" AS fshi
    JOIN public."FileSystemItem" AS fsi ON fshi."ServiceNowKey" = fsi."ServiceNowKey"
    JOIN public."ServerItem" AS si ON fsi."ServerItemServiceNowKey" = si."ServiceNowKey"
    WHERE fshi."CreatedOn" >= $2
      AND fshi."CreatedOn" < last_month_start
      AND ($4 IS NULL OR si."TenantId" = $4)
      AND ($5 IS NULL OR si."OrganizationId" = $5)
      AND ($6 IS NULL OR si."OperatingSystemItemId" = $6)
      AND ($7 IS NULL OR fshi."ServerItemServiceNowKey" = $7)
      AND (si."TenantId" IN (SELECT "TenantId" FROM public."UserTenant" WHERE "UserId" = $1)
        OR si."OrganizationId" IN (SELECT "OrganizationId" FROM public."UserOrganization" WHERE "UserId" = $1))
    UNION
    SELECT fshi.*
      , ROW_NUMBER() OVER (PARTITION BY fshi."ServiceNowKey", EXTRACT(YEAR FROM fshi."CreatedOn"), EXTRACT(MONTH FROM fshi."CreatedOn") ORDER BY fshi."CreatedOn" DESC) AS "rn"
    FROM public."FileSystemHistoryItem" AS fshi
    JOIN public."FileSystemItem" AS fsi ON fshi."ServiceNowKey" = fsi."ServiceNowKey"
    JOIN public."ServerItem" AS si ON fsi."ServerItemServiceNowKey" = si."ServiceNowKey"
    WHERE fshi."InstallStatus" = 1
      AND fshi."CreatedOn" >= last_month_start
      AND ($3 IS NULL OR fshi."CreatedOn" <= $3)
      AND ($4 IS NULL OR si."TenantId" = $4)
      AND ($5 IS NULL OR si."OrganizationId" = $5)
      AND ($6 IS NULL OR si."OperatingSystemItemId" = $6)
      AND ($7 IS NULL OR fshi."ServerItemServiceNowKey" = $7)
      AND (si."TenantId" IN (SELECT "TenantId" FROM public."UserTenant" WHERE "UserId" = $1)
        OR si."OrganizationId" IN (SELECT "OrganizationId" FROM public."UserOrganization" WHERE "UserId" = $1))
  ) AS "sub"
  WHERE "rn" = 1
  ORDER BY "ServiceNowKey", "CreatedOn";
END;$$
