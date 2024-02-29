-- Delete any duplicate file system items.

DELETE FROM public."FileSystemItem"
WHERE "ServiceNowKey" IN (
  SELECT
    "ServiceNowKey"
  FROM (
    SELECT fsi."ServiceNowKey"
      , ROW_NUMBER() OVER (PARTITION BY fsi."ServerItemServiceNowKey", fsi."Name" ORDER BY fsi."CreatedOn" DESC) AS "rn"
    FROM public."FileSystemItem" AS fsi
  ) AS "sub"
  WHERE "rn" != 1
);

-- Delete all Windows servers file system items with the name 'System'.

DELETE FROM public."FileSystemItem"
WHERE "Name" = 'System';

-- Delete server items without any file system items.

DELETE FROM public."ServerItem" si
WHERE NOT EXISTS (
  SELECT FROM public."FileSystemItem" fsi
  WHERE fsi."ServerItemServiceNowKey" = si."ServiceNowKey"
);

-- Remove records that have no related server items.

DELETE FROM public."Tenant" t
WHERE NOT EXISTS (
  SELECT FROM public."ServerItem" si
  WHERE si."TenantId" = t."Id"
);

DELETE FROM public."Organization" o
WHERE NOT EXISTS (
  SELECT FROM public."ServerItem" si
  WHERE si."OrganizationId" = o."Id"
);

DELETE FROM public."OperatingSystemItem" osi
WHERE NOT EXISTS (
  SELECT FROM public."ServerItem" si
  WHERE si."OperatingSystemItemId" = osi."Id"
);
