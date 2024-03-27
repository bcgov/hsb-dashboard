CREATE OR REPLACE FUNCTION "FindServerHistoryItemsByMonth"(
  "startDate" TIMESTAMPTZ
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
	end_date_default := (SELECT COALESCE($2, NOW()));
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
    SELECT *
      , ROW_NUMBER() OVER (PARTITION BY "ServiceNowKey", EXTRACT(YEAR FROM "CreatedOn"), EXTRACT(MONTH FROM "CreatedOn") ORDER BY "CreatedOn" DESC) AS "rn"
    FROM public."ServerHistoryItem"
    WHERE "CreatedOn" >= $1
      AND "CreatedOn" < last_month_start
      AND ($3 IS NULL OR "TenantId" = $3)
      AND ($4 IS NULL OR "OrganizationId" = $4)
      AND ($5 IS NULL OR "OperatingSystemItemId" = $5)
      AND ($6 IS NULL OR "ServiceNowKey" = $6)
    UNION
    SELECT *
      , ROW_NUMBER() OVER (PARTITION BY "ServiceNowKey", EXTRACT(YEAR FROM "CreatedOn"), EXTRACT(MONTH FROM "CreatedOn") ORDER BY "CreatedOn" DESC) AS "rn"
    FROM public."ServerHistoryItem"
    WHERE "InstallStatus" = 1
      AND "CreatedOn" >= last_month_start
      AND ($2 IS NULL OR "CreatedOn" <= $2)
      AND ($3 IS NULL OR "TenantId" = $3)
      AND ($4 IS NULL OR "OrganizationId" = $4)
      AND ($5 IS NULL OR "OperatingSystemItemId" = $5)
      AND ($6 IS NULL OR "ServiceNowKey" = $6)
  ) AS "sub"
  WHERE "rn" = 1
  ORDER BY "ServiceNowKey", "CreatedOn";
END;$$

-- Use by calling
-- select * from public."FindServerHistoryItemsByMonth"('2023-12-01');
