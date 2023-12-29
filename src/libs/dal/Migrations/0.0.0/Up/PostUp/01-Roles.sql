INSERT INTO public."Role" (
  "Key"
  , "Name"
  , "Description"
  , "IsEnabled"
  , "SortOrder"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  gen_random_uuid()
  , 'hsb'
  , 'HSB users'
  , true
  , 0
  , ''
  , ''
), (
  gen_random_uuid()
  , 'system-admin'
  , 'System administrators'
  , true
  , 0
  , ''
  , ''
), (
  gen_random_uuid()
  , 'client'
  , 'Client users'
  , true
  , 0
  , ''
  , ''
), (
  gen_random_uuid()
  , 'organization-admin'
  , 'Client organization administrators'
  , true
  , 0
  , ''
  , ''
), (
  gen_random_uuid()
  , 'service-now'
  , 'Service Account to allow the Data Sync Service to connect to the API.'
  , true
  , 0
  , ''
  , ''
);
