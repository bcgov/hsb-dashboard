UPDATE public."FileSystemHistoryItem" fshi
SET
  "ServerItemServiceNowKey" = fsi."ServerItemServiceNowKey"
FROM (
  SELECT
    "ServiceNowKey"
    , "ServerItemServiceNowKey"
  FROM public."FileSystemItem"
) fsi
WHERE fsi."ServiceNowKey" = fshi."ServiceNowKey";
