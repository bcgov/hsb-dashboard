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
  , 'sys_class_name=cmdb_ci_server^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_win_server^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_solaris_server^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_linux_server^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_unix_server^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_esx_server^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_aix_server^install_status=1' -- Query
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
  , 'sys_class_name=u_cmdb_ci_appliance^install_status=1' -- Query
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
  , 'sys_class_name=cmdb_ci_pc_hardware^install_status=1' -- Query
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
  , 'sys_class_name=u_openvms^install_status=1' -- Query
  , false -- IsActive
  , 0 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
),(
  'cmdb_ci_file_system' -- Name
  , '' -- Description
  , true -- IsEnabled
  , 0 -- Offset
  , 'sys_class_name=cmdb_ci_file_system^install_status=1' -- Query
  , false -- IsActive
  , 99 -- SortOrder
  , '' -- CreatedBy
  , NOW() -- CreatedOn
  , '' -- UpdatedBy
  , NOW() -- UpdatedOn
);
