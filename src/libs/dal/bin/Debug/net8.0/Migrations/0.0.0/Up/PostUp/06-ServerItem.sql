INSERT INTO public."ServerItem" (
  "ConfigurationItemId"
  , "OperatingSystemItemId"
  , "ServiceNowKey"
  , "Name"
  , "Category"
  , "SubCategory"
  , "DiskSpace"
  , "DnsDomain"
  , "SysClassName"
  , "Platform"
  , "IPAddress"
  , "RawData"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  (SELECT "Id" FROM public."ConfigurationItem" WHERE "ServiceNowKey" = 'a2bb92b4db747384fef69878db9619ad') -- ConfigurationItemId
  , (SELECT "Id" FROM public."OperatingSystemItem" WHERE "ServiceNowKey" = '29a97f61db1e9700753d9c78db96199d') -- OperatingSystemItemId
  , 'a2bb92b4db747384fef69878db9619ad' -- ServiceNowKey
  , 'server1' -- Name
  , 'Hardware' -- Category
  , 'Computer' -- SubCategory
  , '130' -- DiskSpace
  , 'hs.advsol.tech' -- DnsDomain
  , 'cmdb_ci_win_server' -- SysClassName
  , 'windows' -- Platform
  , '192.168.40.11' -- IPAddress
  , '{
      "firewall_status": "Intranet",
      "os_address_width": "64",
      "u_arcsight_log_collection": "2022-10-09 07:00:00",
      "x_62100_sovlabs_vra_id": "",
      "attested_date": "",
      "operational_status": "1",
      "u_pci_dss_compliance": "false",
      "u_automation_services_tenant": "",
      "os_service_pack": "",
      "u_model_category": {
          "link": "https://{{instance}}.service-now.com/api/now/table/cmdb_model_category/7900c954c3031000b959fd251eba8f7e",
          "value": "7900c954c3031000b959fd251eba8f7e"
      },
      "cpu_core_thread": "1",
      "cpu_manufacturer": {
          "link": "https://{{instance}}.service-now.com/api/now/table/core_company/86484accdb15f700084171713c961912",
          "value": "86484accdb15f700084171713c961912"
      },
      "sys_updated_on": "2023-05-22 12:30:19",
      "u_related_ticket": "",
      "discovery_source": "ServiceNow",
      "first_discovered": "2019-07-13 05:33:23",
      "due_in": "",
      "u_client_organization": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client_organization/5ba22e17db155780753d9c78db9619e1",
          "value": "5ba22e17db155780753d9c78db9619e1"
      },
      "x_62100_sovlabs_vro_server": "",
      "u_role": "",
      "used_for": "Production",
      "gl_account": "",
      "invoice_number": "",
      "u_operating_system": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_operating_system/29a97f61db1e9700753d9c78db96199d",
          "value": "29a97f61db1e9700753d9c78db96199d"
      },
      "sys_created_by": "anukpe",
      "ram": "8192",
      "warranty_expiration": "",
      "u_tenant_last_changed": "2022-02-02 02:32:28",
      "cpu_name": "Intel(R) Xeon(R) Platinum 8268 CPU @ 2.90GHz",
      "u_server_size": "",
      "cpu_speed": "2893",
      "owned_by": "",
      "checked_out": "",
      "classification": "Production",
      "disk_space": "130",
      "sys_domain_path": "/",
      "u_transitioned": "false",
      "business_unit": "",
      "object_id": "",
      "u_pcm_security_scan_date": "2021-02-19 01:39:00",
      "maintenance_schedule": "",
      "attested_by": "",
      "dns_domain": "hs.advsol.tech",
      "assigned": "",
      "u_reclaimable_memory": "4",
      "life_cycle_stage": "",
      "purchase_date": "",
      "u_restricted_list_reason": "",
      "u_pcm_group": "MTH2",
      "cd_speed": "",
      "short_description": "workgroups",
      "u_active_start_date": "",
      "floppy": "",
      "managed_by": "",
      "os_domain": "idir.BCGOV",
      "u_configured_no_of_vcpus": "4",
      "u_as_sdn_tenant": "",
      "can_print": "false",
      "last_discovered": "2023-05-22 10:33:08",
      "sys_class_name": "cmdb_ci_win_server",
      "u_platform": "windows",
      "u_software_defined_network": "false",
      "cpu_count": "4",
      "manufacturer": {
          "link": "https://{{instance}}.service-now.com/api/now/table/core_company/4aa9ec6edbdc330085539c78db9619e1",
          "value": "4aa9ec6edbdc330085539c78db9619e1"
      },
      "life_cycle_stage_status": "",
      "u_office": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client_office/7d980005db651b80753d9c78db961919",
          "value": "7d980005db651b80753d9c78db961919"
      },
      "vendor": "",
      "u_sdn_tenant": "",
      "u_no_backups": "false",
      "u_reclaimable_vcpus": "2",
      "assigned_to": "",
      "start_date": "",
      "u_active_end_date": "",
      "u_last_patched_date_time": "",
      "u_supported_by_secondary": {
          "link": "https://{{instance}}.service-now.com/api/now/table/sys_user/4e1d5d63db1e57002be89a67db961998",
          "value": "4e1d5d63db1e57002be89a67db961998"
      },
      "x_62100_sovlabs_vra_machine_id": "",
      "os_version": "10.0.14393",
      "u_pre_approved_by": "",
      "u_discovery_exemption": "",
      "serial_number": "VMware-42 03 cf 71 1e 76 1f 8e-4f 73 85 f3 f2 87 92 bb",
      "u_special_processing": "false",
      "u_delayed_by": "",
      "cd_rom": "false",
      "u_configured_amount_of_ram": "8",
      "support_group": {
          "link": "https://{{instance}}.service-now.com/api/now/table/sys_user_group/d5be82aedb1ad70003399274db961957",
          "value": "d5be82aedb1ad70003399274db961957"
      },
      "u_ci_sync_last_sync_date": "2023-05-22 12:30:19",
      "correlation_id": "",
      "u_pre_approval": "false",
      "unverified": "false",
      "attributes": "",
      "u_approval_groups": "",
      "asset": {
          "link": "https://{{instance}}.service-now.com/api/now/table/alm_asset/029b1674db747384fef69878db96190e",
          "value": "029b1674db747384fef69878db96190e"
      },
      "u_last_patched_job_num": "",
      "cpu_core_count": "1",
      "form_factor": "",
      "u_on_call_group": "Winhost-GRP1",
      "x_fru_foundation_device_function": {
          "link": "https://{{instance}}.service-now.com/api/now/table/x_fru_foundation_device_function/a2bd8cc5db5ad70001509c78db9619d0",
          "value": "a2bd8cc5db5ad70001509c78db9619d0"
      },
      "skip_sync": "false",
      "u_rtsm_id": "4b07b616c611fa0f877f5ba880c69f2d",
      "u_pre_approved_until": "",
      "most_frequent_user": "",
      "attestation_score": "",
      "u_delayed_from_next_patch": "false",
      "sys_updated_by": "CaseExchange",
      "sys_created_on": "2019-04-11 16:50:13",
      "cpu_type": "GenuineIntel",
      "sys_domain": {
          "link": "https://{{instance}}.service-now.com/api/now/table/sys_user_group/global",
          "value": "global"
      },
      "install_date": "2019-04-11 22:42:37",
      "u_logical_asset_system": "N/A",
      "asset_tag": "",
      "dr_backup": "",
      "u_arcsight_log_exclusion": "false",
      "u_ci_sync_sys_id": "71eb1e38dbfc734c2ade9a26db961928",
      "u_patch_notification_contacts": "",
      "hardware_substatus": "",
      "u_client_ci_name": "server1.idir.bcgov",
      "fqdn": "server1.idir.BCGOV",
      "change_control": "",
      "u_sla_tier_numbers": "f43294bbdbf55f0003399274db9619d1",
      "internet_facing": "true",
      "u_delayed_reason": "",
      "delivery_date": "",
      "hardware_status": "installed",
      "install_status": "1",
      "supported_by": {
          "link": "https://{{instance}}.service-now.com/api/now/table/sys_user/af1d11a3db1e57002be89a67db961910",
          "value": "af1d11a3db1e57002be89a67db961910"
      },
      "name": "server1",
      "u_asset_id_number": "ASTID0000013915",
      "u_sequence_info": "",
      "u_client": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client/0beaa1a9db5d5380753d9c78db961947",
          "value": "0beaa1a9db5d5380753d9c78db961947"
      },
      "u_tenant": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_tenant/b619b71e1b2bb0103c1eec29b04bcb1d",
          "value": "b619b71e1b2bb0103c1eec29b04bcb1d"
      },
      "subcategory": "Computer",
      "default_gateway": "192.168.8.1",
      "chassis_type": "Other",
      "virtual": "true",
      "assignment_group": "",
      "u_rack_location": "",
      "u_as_uplift": "",
      "managed_by_group": "",
      "u_parent_vcenter": "STMS",
      "u_customer_contact": "Brian Price",
      "u_os_storage": "",
      "sys_id": "a2bb92b4db747384fef69878db9619ad",
      "u_automation_services": "false",
      "cluster_id": "",
      "po_number": "",
      "u_on_restricted_list": "false",
      "checked_in": "",
      "sys_class_path": "/!!/!2/!(/!!/!#",
      "mac_address": "",
      "u_special_processing_instructions": "",
      "u_ad_group": "",
      "company": "",
      "justification": "",
      "department": "",
      "u_after_hours_support_contact": "",
      "cluster_name": "",
      "u_cluster": "",
      "comments": "server1 was added to SDN Network - 2021-04-28 - RITM0091878",
      "cost": "",
      "attestation_status": "Not Yet Reviewed",
      "sys_mod_count": "1575",
      "u_pcm_ip_address": "192.168.115.25",
      "monitor": "false",
      "ip_address": "192.168.40.11",
      "model_id": {
          "link": "https://{{instance}}.service-now.com/api/now/table/cmdb_model/4aba07aadb3adf405d8e90f9db961924",
          "value": "4aba07aadb3adf405d8e90f9db961924"
      },
      "duplicate_of": "",
      "sys_tags": "",
      "u_sdn": "false",
      "cost_cc": "USD",
      "order_date": "",
      "schedule": "",
      "u_primary_node_name": "server1-m.hs.advsol.tech",
      "environment": "",
      "due": "",
      "attested": "false",
      "location": {
          "link": "https://{{instance}}.service-now.com/api/now/table/cmn_location/46f055dedbca934003399274db96193d",
          "value": "46f055dedbca934003399274db96193d"
      },
      "u_technical_contact": "Brian Price",
      "category": "Hardware",
      "fault_count": "0",
      "host_name": "server1",
      "lease_id": "",
      "u_delayed_until": ""
    }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
);
