INSERT INTO public."GroupRole" (
  "GroupId"
  , "RoleId"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  (SELECT "Id" FROM public."Group" WHERE "Name" = 'hsb' LIMIT 1)
  , (SELECT "Id" FROM public."Role" WHERE "Name" = 'hsb' LIMIT 1)
  , ''
  , ''
), (
  (SELECT "Id" FROM public."Group" WHERE "Name" = 'system-admin' LIMIT 1)
  , (SELECT "Id" FROM public."Role" WHERE "Name" = 'system-admin' LIMIT 1)
  , ''
  , ''
), (
  (SELECT "Id" FROM public."Group" WHERE "Name" = 'client' LIMIT 1)
  , (SELECT "Id" FROM public."Role" WHERE "Name" = 'client' LIMIT 1)
  , ''
  , ''
), (
  (SELECT "Id" FROM public."Group" WHERE "Name" = 'organization-admin' LIMIT 1)
  , (SELECT "Id" FROM public."Role" WHERE "Name" = 'organization-admin' LIMIT 1)
  , ''
  , ''
), (
  (SELECT "Id" FROM public."Group" WHERE "Name" = 'service-now' LIMIT 1)
  , (SELECT "Id" FROM public."Role" WHERE "Name" = 'service-now' LIMIT 1)
  , ''
  , ''
);
