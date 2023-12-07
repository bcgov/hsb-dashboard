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
  'Ministry of Citizens'' Services' -- name
  , 'CITZ' -- Code
  , 1 -- OrganizationType
  , (SELECT "Id" FROM public."Organization" WHERE "Code" = 'SECT-01') -- ParentId
  , '5ba22e17db155780753d9c78db9619e1' -- ServiceNowKey
  , '' -- Description
  , true -- IsEnabled
  , 0 -- SortOrder
  , '{
      "u_name": "Ministry of Citizens'' Services",
      "sys_mod_count": "5",
      "sys_updated_on": "2021-03-31 02:14:40",
      "sys_tags": "",
      "u_as_client": {
          "link": "https://{{instance}}.service-now.com/api/now/table/u_as_client/0beaa1a9db5d5380753d9c78db961947",
          "value": "0beaa1a9db5d5380753d9c78db961947"
      },
      "sys_id": "5ba22e17db155780753d9c78db9619e1",
      "u_available_in_processes": "71b4e272db1a5b00753d9c78db96191e,08c4e272db1a5b00753d9c78db96196b,5dc42272db1a5b00753d9c78db96194c,65d46672db1a5b00753d9c78db961925,a4f4e272db1a5b00753d9c78db961975,ffc42e32db1a5b00753d9c78db9619c7",
      "sys_updated_by": "user_a",
      "u_billing_rate_type": "Standard",
      "sys_created_on": "2018-04-11 15:54:29",
      "u_active": "true",
      "u_org_code": "CITZ",
      "u_billing_sku_type": "iStore",
      "sys_created_by": "email@provider.com",
      "x_msces_billing_org_rate_type": "iStore"
  }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
);
