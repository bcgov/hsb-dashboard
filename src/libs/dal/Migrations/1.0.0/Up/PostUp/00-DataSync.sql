INSERT INTO public."DataSync" (
  "Name"
  , "Description"
  , "IsEnabled"
  , "DataType"
  , "Offset"
  , "Query"
  , "CreatedBy"
  , "CreatedOn"
  , "UpdatedBy"
  , "UpdatedOn"
) VALUES (
  'Server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- DataType
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_server' -- Query
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'FileSystem' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 1 -- DataType
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_file_system' -- Query
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
);
