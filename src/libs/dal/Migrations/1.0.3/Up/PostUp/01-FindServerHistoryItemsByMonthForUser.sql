CREATE OR REPLACE FUNCTION "FindServerHistoryItemsByMonthForUser"(
  "userId" INT
  , "startDate" TIMESTAMPTZ
  , "endDate" TIMESTAMPTZ DEFAULT NULL
  , "tenantId" INT DEFAULT NULL
  , "organizationId" INT DEFAULT NULL
  , "operatingSystemItemId" INT DEFAULT NULL
  , "serviceNowKey" VARCHAR(200) DEFAULT NULL
)
RETURNS SETOF public."ServerHistoryItem"
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
    , "TenantId"
    , "OrganizationId"
    , "OperatingSystemItemId"
    , "ServiceNowKey"
    , "HistoryKey"
    , "RawData"
    , "RawDataCI"
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
  FROM (
    SELECT shi.*
      , ROW_NUMBER() OVER (PARTITION BY shi."ServiceNowKey", EXTRACT(YEAR FROM shi."CreatedOn"), EXTRACT(MONTH FROM shi."CreatedOn") ORDER BY shi."CreatedOn" DESC) AS "rn"
    FROM public."ServerHistoryItem" shi
    JOIN public."ServerItem" si ON shi."ServiceNowKey" = si."ServiceNowKey"
    WHERE shi."CreatedOn" >= $2
      AND shi."CreatedOn" < last_month_start
      AND ($4 IS NULL OR shi."TenantId" = $4)
      AND ($5 IS NULL OR shi."OrganizationId" = $5)
      AND ($6 IS NULL OR shi."OperatingSystemItemId" = $6)
      AND ($7 IS NULL OR shi."ServiceNowKey" = $7)
      AND (si."TenantId" IN (SELECT "TenantId" FROM public."UserTenant" WHERE "UserId" = $1)
        OR si."OrganizationId" IN (SELECT "OrganizationId" FROM public."UserOrganization" WHERE "UserId" = $1))
    UNION
    SELECT shi.*
      , ROW_NUMBER() OVER (PARTITION BY shi."ServiceNowKey", EXTRACT(YEAR FROM shi."CreatedOn"), EXTRACT(MONTH FROM shi."CreatedOn") ORDER BY shi."CreatedOn" DESC) AS "rn"
    FROM public."ServerHistoryItem" shi
    JOIN public."ServerItem" si ON shi."ServiceNowKey" = si."ServiceNowKey"
    WHERE shi."InstallStatus" = 1
      AND shi."CreatedOn" >= last_month_start
      AND ($3 IS NULL OR shi."CreatedOn" <= $3)
      AND ($4 IS NULL OR shi."TenantId" = $4)
      AND ($5 IS NULL OR shi."OrganizationId" = $5)
      AND ($6 IS NULL OR shi."OperatingSystemItemId" = $6)
      AND ($7 IS NULL OR shi."ServiceNowKey" = $7)
      AND (si."TenantId" IN (SELECT "TenantId" FROM public."UserTenant" WHERE "UserId" = $1)
        OR si."OrganizationId" IN (SELECT "OrganizationId" FROM public."UserOrganization" WHERE "UserId" = $1))
  ) AS "sub"
  WHERE "rn" = 1
  ORDER BY "ServiceNowKey", "CreatedOn";
END;$$

-- Use by calling
-- select * from public."FindServerHistoryItemsByMonthForUser"(1, '2023-12-01');
