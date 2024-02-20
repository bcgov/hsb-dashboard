UPDATE public."FileSystemHistoryItem" fshi
SET
  "InstallStatus" = fsi."InstallStatus"
FROM (
  SELECT
    "Id"
    , ("RawData"->>'install_status')::int AS "InstallStatus"
  FROM public."FileSystemHistoryItem"
) fsi
WHERE fsi."Id" = fshi."Id";
