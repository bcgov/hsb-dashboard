UPDATE public."ServerItem" fshi
SET
  "InstallStatus" = fsi."InstallStatus"
FROM (
  SELECT
    "ServiceNowKey"
    , ("RawData"->>'install_status')::int AS "InstallStatus"
  FROM public."ServerItem"
) fsi
WHERE fsi."ServiceNowKey" = fshi."ServiceNowKey";
