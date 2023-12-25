INSERT INTO public."DataSync" (
  "Name"
  , "Description"
  , "IsEnabled"
  , "Offset"
  , "Query"
  , "IsActive"
  , "SortOrder"
  , "CreatedBy"
  , "CreatedOn"
  , "UpdatedBy"
  , "UpdatedOn"
) VALUES (
  'cmdb_ci_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_win_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_win_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_solaris_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_solaris_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_linux_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_linux_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_unix_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_unix_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_esx_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_esx_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_aix_server' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_aix_server' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'u_cmdb_ci_appliance' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=u_cmdb_ci_appliance' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_pc_hardware' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_pc_hardware' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'u_openvms' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=u_openvms' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'FileSystem' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_file_system' -- Query
  , false -- IsActive
  , 99 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
);
