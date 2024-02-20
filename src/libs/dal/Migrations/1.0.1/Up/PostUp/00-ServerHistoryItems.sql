UPDATE public."ServerHistoryItem" fshi
SET
  "InstallStatus" = fsi."InstallStatus"
FROM (
  SELECT
    "Id"
    , ("RawData"->>'install_status')::int AS "InstallStatus"
  FROM public."ServerHistoryItem"
) fsi
WHERE fsi."Id" = fshi."Id";
