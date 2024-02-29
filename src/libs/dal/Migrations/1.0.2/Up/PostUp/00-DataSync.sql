-- Disabling data sync tables to speed up the process.
-- These are not likely needed as anything with a file system item would get picked up by that table process anyway.

UPDATE public."DataSync"
SET "IsEnabled" = false
WHERE "Name" != 'cmdb_ci_file_system';

UPDATE public."DataSync"
SET "Query" = 'sys_class_name=cmdb_ci_file_system^install_status=1'
WHERE "Name" = 'cmdb_ci_file_system';
