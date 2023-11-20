INSERT INTO public."OperatingSystemItem" (
  "UName"
  , "ServiceNowKey"
  , "RawData"
  , "CreatedBy"
  , "UpdatedBy"
) VALUES (
  'windows 2016 server standard edition' -- UName
  , '29a97f61db1e9700753d9c78db96199d' -- ServiceNowKey
  , '{
        "u_name": "windows 2016 server standard edition",
        "u_discovered_os": "Windows 2016 Server Standard",
        "u_end_of_life": "2027-01-12",
        "sys_mod_count": "8",
        "sys_updated_on": "2022-04-28 18:07:11",
        "sys_tags": "",
        "u_discovered_os_version": "10.0.14393",
        "sys_id": "29a97f61db1e9700753d9c78db96199d",
        "sys_updated_by": "user_a",
        "u_class": "cmdb_ci_win_server",
        "sys_created_on": "2018-05-26 04:47:53",
        "u_active": "true",
        "u_discovered_os_sn": "Windows 2016 Standard",
        "sys_created_by": "user@email.com"
    }' -- RawData
  , '' -- CreatedBy
  , '' -- UpdatedBy
);
