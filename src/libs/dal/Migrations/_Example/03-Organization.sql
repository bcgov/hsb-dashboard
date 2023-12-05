INSERT INTO public."Organization" (
  "Name"
  , "Code"
  , "OrganizationType"
  , "ParentId"
  , "ServiceNowKey"
  , "Description"
  , "IsEnabled"
  , "SortOrder"
  , "RawData"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  'CITZ-Hosting' -- Name
  , 'HSB' -- Code
  , 0 -- OrganizationType
  , (SELECT "Id" FROM public."Organization" WHERE "ServiceNowKey" = '5ba22e17db155780753d9c78db9619e1') -- ParentId
  , 'b619b71e1b2bb0103c1eec29b04bcb1d' -- ServiceNowKey
  , '' -- Description
  , true -- IsEnabled
  , 0 -- SortOrder
  , '{
      "u_name": "CITZ-Hosting",
      "u_client_organizations": "5ba22e17db155780753d9c78db9619e1",
      "sys_mod_count": "2",
      "active": "true",
      "sys_updated_on": "2022-04-20 01:28:03",
      "sys_tags": "",
      "sys_class_name": "u_tenant",
      "sys_id": "b619b71e1b2bb0103c1eec29b04bcb1d",
      "sys_package": {
          "link": "https://thehubtest.service-now.com/api/now/table/sys_package/global",
          "value": "global"
      },
      "sys_update_name": "u_tenant_b619b71e1b2bb0103c1eec29b04bcb1d",
      "sys_updated_by": "user_a",
      "sys_created_on": "2021-11-03 17:06:22",
      "u_sdn_tenant_license_plate": "",
      "sys_name": "CITZ-Hosting",
      "u_client": {
          "link": "https://thehubtest.service-now.com/api/now/table/u_as_client/0beaa1a9db5d5380753d9c78db961947",
          "value": "0beaa1a9db5d5380753d9c78db961947"
      },
      "u_type": "vROps",
      "sys_created_by": "user_a",
      "order": "100",
      "sys_policy": ""
    }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
);
