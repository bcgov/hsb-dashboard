INSERT INTO public."ConfigurationItem" (
  "OrganizationId"
  , "ServiceNowKey"
  , "Name"
  , "Category"
  , "SubCategory"
  , "UPlatform"
  , "DnsDomain"
  , "SysClassName"
  , "FQDN"
  , "IPAddress"
  , "RawData"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  (SELECT "Id" FROM public."Organization" WHERE "ServiceNowKey" = 'b619b71e1b2bb0103c1eec29b04bcb1d') -- OrganizationId
  , 'a2bb92b4db747384fef69878db9619ad' -- ServiceNowKey
  , 'server1' -- Name
  , 'Hardware' -- Category
  , 'Computer' -- SubCategory
  , 'windows' -- UPlatform
  , 'hs.advsol.tech' -- DnsDomain
  , 'cmdb_ci_win_server' -- SysClassName
  , 'server1.idir.BCGOV' -- FQDN
  , '192.168.40.11' -- IPAddress
  , '{
        "x_62100_sovlabs_vra_id": "",
        "attested_date": "",
        "operational_status": "1",
        "u_automation_services_tenant": "",
        "u_model_category": {
            "link": "https://{{instance}}.service-now.com/api/now/table/cmdb_model_category/7900c954c3031000b959fd251eba8f7e",
            "value": "7900c954c3031000b959fd251eba8f7e"
        },
        "sys_updated_on": "2023-05-22 12:30:19",
        "discovery_source": "ServiceNow",
        "first_discovered": "2019-07-13 05:33:23",
        "due_in": "",
        "u_client_organization": {
            "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client_organization/5ba22e17db155780753d9c78db9619e1",
            "value": "5ba22e17db155780753d9c78db9619e1"
        },
        "x_62100_sovlabs_vro_server": "",
        "u_role": "",
        "gl_account": "",
        "invoice_number": "",
        "sys_created_by": "anukpe",
        "warranty_expiration": "",
        "u_tenant_last_changed": "2022-01-01 00:00:00",
        "owned_by": "",
        "checked_out": "",
        "sys_domain_path": "/",
        "business_unit": "",
        "maintenance_schedule": "",
        "attested_by": "",
        "dns_domain": "hs.advsol.tech",
        "assigned": "",
        "life_cycle_stage": "",
        "purchase_date": "",
        "short_description": "workgroups",
        "u_active_start_date": "",
        "managed_by": "",
        "u_as_sdn_tenant": "",
        "can_print": "false",
        "last_discovered": "2023-05-22 10:33:08",
        "sys_class_name": "cmdb_ci_win_server",
        "u_platform": "windows",
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
        "u_no_backups": "false",
        "assigned_to": "",
        "start_date": "",
        "u_active_end_date": "",
        "u_supported_by_secondary": {
            "link": "https://{{instance}}.service-now.com/api/now/table/sys_user/4e1d5d63db1e57002be89a67db961998",
            "value": "4e1d5d63db1e57002be89a67db961998"
        },
        "x_62100_sovlabs_vra_machine_id": "",
        "serial_number": "VMware-42 03 cf 71 1e 76 1f 8e-4f 73 85 f3 f2 87 92 bb",
        "support_group": {
            "link": "https://{{instance}}.service-now.com/api/now/table/sys_user_group/d5be82aedb1ad70003399274db961957",
            "value": "d5be82aedb1ad70003399274db961957"
        },
        "u_ci_sync_last_sync_date": "2020-01-01 00:00:00",
        "correlation_id": "",
        "unverified": "false",
        "attributes": "",
        "u_approval_groups": "",
        "asset": {
            "link": "https://{{instance}}.service-now.com/api/now/table/alm_asset/029b1674db747384fef69878db96190e",
            "value": "029b1674db747384fef69878db96190e"
        },
        "x_fru_foundation_device_function": {
            "link": "https://{{instance}}.service-now.com/api/now/table/x_fru_foundation_device_function/a2bd8cc5db5ad70001509c78db9619d0",
            "value": "a2bd8cc5db5ad70001509c78db9619d0"
        },
        "skip_sync": "false",
        "attestation_score": "",
        "sys_updated_by": "CaseExchange",
        "sys_created_on": "2020-01-01 00:00:00",
        "sys_domain": {
            "link": "https://{{instance}}.service-now.com/api/now/table/sys_user_group/global",
            "value": "global"
        },
        "install_date": "2020-01-01 00:00:00",
        "u_logical_asset_system": "N/A",
        "asset_tag": "",
        "u_ci_sync_sys_id": "71eb1e38dbfc734c2ade9a26db961928",
        "u_patch_notification_contacts": "",
        "u_client_ci_name": "server1.idir.bcgov",
        "fqdn": "server1.idir.BCGOV",
        "change_control": "",
        "u_sla_tier_numbers": "f43294bbdbf55f0003399274db9619d1",
        "delivery_date": "",
        "install_status": "1",
        "supported_by": {
            "link": "https://{{instance}}.service-now.com/api/now/table/sys_user/af1d11a3db1e57002be89a67db961910",
            "value": "af1d11a3db1e57002be89a67db961910"
        },
        "name": "server1",
        "u_asset_id_number": "ASTID0000013915",
        "u_client": {
            "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client/0beaa1a9db5d5380753d9c78db961947",
            "value": "0beaa1a9db5d5380753d9c78db961947"
        },
        "u_tenant": {
            "link": "https://{{instance}}.service-now.com/api/now/table/u_tenant/b619b71e1b2bb0103c1eec29b04bcb1d",
            "value": "b619b71e1b2bb0103c1eec29b04bcb1d"
        },
        "subcategory": "Computer",
        "assignment_group": "",
        "u_rack_location": "",
        "u_as_uplift": "",
        "managed_by_group": "",
        "u_customer_contact": "Brian Price",
        "sys_id": "a2bb92b4db747384fef69878db9619ad",
        "u_automation_services": "false",
        "po_number": "",
        "checked_in": "",
        "sys_class_path": "/!!/!2/!(/!!/!#",
        "mac_address": "",
        "u_ad_group": "",
        "company": "",
        "justification": "",
        "department": "",
        "u_after_hours_support_contact": "",
        "u_cluster": "",
        "comments": "Server1 was added to SDN Network",
        "cost": "",
        "attestation_status": "Not Yet Reviewed",
        "sys_mod_count": "1575",
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
        "lease_id": ""
    }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
);
