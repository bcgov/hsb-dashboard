UPDATE public."FileSystemItem" fshi
SET
  "InstallStatus" = fsi."InstallStatus"
FROM (
  SELECT
    "ServiceNowKey"
    , ("RawData"->>'install_status')::int AS "InstallStatus"
  FROM public."FileSystemItem"
) fsi
WHERE fsi."ServiceNowKey" = fshi."ServiceNowKey";
