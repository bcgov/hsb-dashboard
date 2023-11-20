INSERT INTO public."FileSystemItem" (
  "ConfigurationItemId"
  , "ServiceNowKey"
  , "Name"
  , "Label"
  , "Category"
  , "SubCategory"
  , "StorageType"
  , "MediaType"
  , "SysClassName"
  , "VolumeId"
  , "Capacity"
  , "DiskSpace"
  , "Size"
  , "SizeBytes"
  , "UsedSizeBytes"
  , "AvailableSpace"
  , "FreeSpace"
  , "FreeSpaceBytes"
  , "RawData"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  (SELECT "Id" FROM public."ConfigurationItem" WHERE "ServiceNowKey" = 'a2bb92b4db747384fef69878db9619ad') -- ConfigurationItemId
  , 'a33f0df5db264010084171713c961967' -- ServiceNowKey
  , 'D' -- Name
  , '' -- Label
  , 'Resource' -- Category
  , 'File Share' -- SubCategory
  , 'logical' -- StorageType
  , 'cd' -- MediaType
  , 'cmdb_ci_file_system' -- SysClassName
  , 'D' -- VolumeId
  , '0' -- Capacity
  , '0' -- DiskSpace
  , '0.0 B' -- Size
  , '0' -- SizeBytes
  , '' -- UsedSizeBytes
  , '0' -- AvailableSpace
  , '0.0 B' -- FreeSpace
  , '0' -- FreeSpaceBytes
  , '{
      "dataset_type": "",
      "storage_type": "logical",
      "x_62100_sovlabs_vra_id": "",
      "attested_date": "",
      "operational_status": "1",
      "lun": "",
      "snapshot_count": "",
      "u_automation_services_tenant": "",
      "u_model_category": "",
      "sharable": "false",
      "sys_updated_on": "2023-05-22 09:49:26",
      "type": "",
      "discovery_source": "ServiceNow",
      "first_discovered": "2020-01-25 09:51:01",
      "due_in": "",
      "u_client_organization": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client_organization/5ba22e17db155780753d9c78db9619e1",
          "value": "5ba22e17db155780753d9c78db9619e1"
      },
      "x_62100_sovlabs_vro_server": "",
      "media_type": "cd",
      "export_name": "",
      "u_role": "",
      "state": "",
      "gl_account": "",
      "invoice_number": "",
      "sys_created_by": "discovery",
      "warranty_expiration": "",
      "u_tenant_last_changed": "",
      "effective_sla_domain_name": "",
      "owned_by": "",
      "checked_out": "",
      "disk_space": "0",
      "sys_domain_path": "/",
      "business_unit": "",
      "object_id": "",
      "maintenance_schedule": "",
      "size": "0.0 B",
      "attested_by": "",
      "dns_domain": "",
      "provided_by": "",
      "assigned": "",
      "life_cycle_stage": "",
      "purchase_date": "",
      "device": "",
      "short_description": "",
      "size_bytes": "0",
      "u_active_start_date": "",
      "volume_id": "D",
      "managed_by": "",
      "u_as_sdn_tenant": "",
      "can_print": "false",
      "last_discovered": "2023-05-22 09:49:26",
      "sys_class_name": "cmdb_ci_file_system",
      "u_platform": "",
      "capacity": "0",
      "manufacturer": "",
      "computer": {
          "link": "https://{{instance}}.service-now.com/api/now/table/cmdb_ci/a2bb92b4db747384fef69878db9619ad",
          "value": "a2bb92b4db747384fef69878db9619ad"
      },
      "life_cycle_stage_status": "",
      "u_office": "",
      "vendor": "",
      "u_no_backups": "false",
      "model_number": "",
      "assigned_to": "",
      "start_date": "",
      "u_active_end_date": "",
      "u_supported_by_secondary": "",
      "x_62100_sovlabs_vra_machine_id": "",
      "serial_number": "",
      "pool_id": "",
      "support_group": "",
      "protected_by": "",
      "u_ci_sync_last_sync_date": "",
      "correlation_id": "",
      "unverified": "false",
      "attributes": "",
      "u_approval_groups": "",
      "asset": "",
      "free_space": "0.0 B",
      "x_fru_foundation_device_function": "",
      "skip_sync": "false",
      "attestation_score": "",
      "sys_updated_by": "discovery",
      "sys_created_on": "2020-01-25 09:51:01",
      "sys_domain": {
          "link": "https://{{instance}}.service-now.com/api/now/table/sys_user_group/global",
          "value": "global"
      },
      "install_date": "",
      "u_logical_asset_system": "",
      "asset_tag": "",
      "u_ci_sync_sys_id": "",
      "u_patch_notification_contacts": "",
      "u_client_ci_name": "",
      "used_size_bytes": "",
      "fqdn": "",
      "change_control": "",
      "u_sla_tier_numbers": "",
      "vdisk_id": "",
      "delivery_date": "",
      "install_status": "1",
      "parent_id": "",
      "supported_by": "",
      "name": "D",
      "u_asset_id_number": "",
      "u_client": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client/0beaa1a9db5d5380753d9c78db961947",
          "value": "0beaa1a9db5d5380753d9c78db961947"
      },
      "delete_on_termination": "false",
      "u_tenant": "",
      "subcategory": "File Share",
      "server_name": "",
      "assignment_group": "",
      "u_rack_location": "",
      "u_as_uplift": "",
      "effective_sla_domain": "",
      "available_space": "0",
      "managed_by_group": "",
      "u_customer_contact": "",
      "mapping_type": "",
      "sys_id": "a33f0df5db264010084171713c961967",
      "u_automation_services": "false",
      "cluster_id": "",
      "file_system": "",
      "po_number": "",
      "checked_in": "",
      "sys_class_path": "/!!/!K/!!",
      "mac_address": "",
      "u_ad_group": "",
      "company": "",
      "justification": "",
      "department": "",
      "u_after_hours_support_contact": "",
      "cluster_name": "",
      "u_cluster": "",
      "comments": "",
      "cost": "",
      "attestation_status": "Not Yet Reviewed",
      "mount_point": "D:\\",
      "sys_mod_count": "1193",
      "monitor": "false",
      "ip_address": "",
      "label": "",
      "model_id": "",
      "duplicate_of": "",
      "sys_tags": "",
      "u_sdn": "false",
      "volume_container": "",
      "cost_cc": "USD",
      "share_count": "",
      "order_date": "",
      "schedule": "",
      "environment": "",
      "share_type": "",
      "due": "",
      "provisioning_type": "",
      "attested": "false",
      "location": "",
      "u_technical_contact": "",
      "category": "Resource",
      "fault_count": "0",
      "free_space_bytes": "0",
      "lease_id": ""
  }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
), (
  (SELECT "Id" FROM public."ConfigurationItem" WHERE "ServiceNowKey" = 'a2bb92b4db747384fef69878db9619ad') -- ConfigurationItemId
  , '2f3f0df5db264010084171713c961967' -- ServiceNowKey
  , 'E' -- Name
  , '' -- Label
  , 'Resource' -- Category
  , 'File Share' -- SubCategory
  , 'logical' -- StorageType
  , 'cd' -- MediaType
  , 'cmdb_ci_file_system' -- SysClassName
  , 'E' -- VolumeId
  , '51070' -- Capacity
  , '049.87' -- DiskSpace
  , '49.9 GB' -- Size
  , '53550772224' -- SizeBytes
  , '' -- UsedSizeBytes
  , '22655' -- AvailableSpace
  , '22.1 GB' -- FreeSpace
  , '23755960320' -- FreeSpaceBytes
  , '{
      "dataset_type": "",
      "storage_type": "logical",
      "x_62100_sovlabs_vra_id": "",
      "attested_date": "",
      "operational_status": "1",
      "lun": "",
      "snapshot_count": "",
      "u_automation_services_tenant": "",
      "u_model_category": "",
      "sharable": "false",
      "sys_updated_on": "2023-05-22 09:49:26",
      "type": "",
      "discovery_source": "ServiceNow",
      "first_discovered": "2020-01-25 09:51:01",
      "due_in": "",
      "u_client_organization": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client_organization/5ba22e17db155780753d9c78db9619e1",
          "value": "5ba22e17db155780753d9c78db9619e1"
      },
      "x_62100_sovlabs_vro_server": "",
      "media_type": "fixed",
      "export_name": "",
      "u_role": "",
      "state": "",
      "gl_account": "",
      "invoice_number": "",
      "sys_created_by": "discovery",
      "warranty_expiration": "",
      "u_tenant_last_changed": "",
      "effective_sla_domain_name": "",
      "owned_by": "",
      "checked_out": "",
      "disk_space": "49.87",
      "sys_domain_path": "/",
      "business_unit": "",
      "object_id": "",
      "maintenance_schedule": "",
      "size": "49.9 GB",
      "attested_by": "",
      "dns_domain": "",
      "provided_by": {
          "link": "https://{{instance}}.service-now.com/api/now/table/cmdb_ci/02b9713987ed70100015ff78cebb35d2",
          "value": "02b9713987ed70100015ff78cebb35d2"
      },
      "assigned": "",
      "life_cycle_stage": "",
      "purchase_date": "",
      "device": "",
      "short_description": "",
      "size_bytes": "53550772224",
      "u_active_start_date": "",
      "volume_id": "E",
      "managed_by": "",
      "u_as_sdn_tenant": "",
      "can_print": "false",
      "last_discovered": "2023-05-22 09:49:26",
      "sys_class_name": "cmdb_ci_file_system",
      "u_platform": "",
      "capacity": "51070",
      "manufacturer": "",
      "computer": {
          "link": "https://{{instance}}.service-now.com/api/now/table/cmdb_ci/a2bb92b4db747384fef69878db9619ad",
          "value": "a2bb92b4db747384fef69878db9619ad"
      },
      "life_cycle_stage_status": "",
      "u_office": "",
      "vendor": "",
      "u_no_backups": "false",
      "model_number": "",
      "assigned_to": "",
      "start_date": "",
      "u_active_end_date": "",
      "u_supported_by_secondary": "",
      "x_62100_sovlabs_vra_machine_id": "",
      "serial_number": "",
      "pool_id": "",
      "support_group": "",
      "protected_by": "",
      "u_ci_sync_last_sync_date": "",
      "correlation_id": "",
      "unverified": "false",
      "attributes": "",
      "u_approval_groups": "",
      "asset": "",
      "free_space": "22.1 GB",
      "x_fru_foundation_device_function": "",
      "skip_sync": "false",
      "attestation_score": "",
      "sys_updated_by": "discovery",
      "sys_created_on": "2020-01-25 09:51:01",
      "sys_domain": {
          "link": "https://{{instance}}.service-now.com/api/now/table/sys_user_group/global",
          "value": "global"
      },
      "install_date": "",
      "u_logical_asset_system": "",
      "asset_tag": "",
      "u_ci_sync_sys_id": "",
      "u_patch_notification_contacts": "",
      "u_client_ci_name": "",
      "used_size_bytes": "",
      "fqdn": "",
      "change_control": "",
      "u_sla_tier_numbers": "",
      "vdisk_id": "",
      "delivery_date": "",
      "install_status": "1",
      "parent_id": "",
      "supported_by": "",
      "name": "E",
      "u_asset_id_number": "",
      "u_client": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client/0beaa1a9db5d5380753d9c78db961947",
          "value": "0beaa1a9db5d5380753d9c78db961947"
      },
      "delete_on_termination": "false",
      "u_tenant": "",
      "subcategory": "File Share",
      "server_name": "",
      "assignment_group": "",
      "u_rack_location": "",
      "u_as_uplift": "",
      "effective_sla_domain": "",
      "available_space": "22655",
      "managed_by_group": "",
      "u_customer_contact": "",
      "mapping_type": "",
      "sys_id": "2f3f0df5db264010084171713c961967",
      "u_automation_services": "false",
      "cluster_id": "",
      "file_system": "ntfs",
      "po_number": "",
      "checked_in": "",
      "sys_class_path": "/!!/!K/!!",
      "mac_address": "",
      "u_ad_group": "",
      "company": "",
      "justification": "",
      "department": "",
      "u_after_hours_support_contact": "",
      "cluster_name": "",
      "u_cluster": "",
      "comments": "",
      "cost": "",
      "attestation_status": "Not Yet Reviewed",
      "mount_point": "E:\\",
      "sys_mod_count": "1193",
      "monitor": "false",
      "ip_address": "",
      "label": "Data_T2",
      "model_id": "",
      "duplicate_of": "",
      "sys_tags": "",
      "u_sdn": "false",
      "volume_container": "",
      "cost_cc": "USD",
      "share_count": "",
      "order_date": "",
      "schedule": "",
      "environment": "",
      "share_type": "",
      "due": "",
      "provisioning_type": "",
      "attested": "false",
      "location": "",
      "u_technical_contact": "",
      "category": "Resource",
      "fault_count": "0",
      "free_space_bytes": "23755960320",
      "lease_id": ""
  }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
);
