import json
import json
import os
from dotenv import load_dotenv
import requests

load_dotenv()

SN_INSTANCE_NAME = os.getenv('SN_INSTANCE_NAME')
SN_USERNAME = os.getenv('SN_USERNAME')
SN_PASSWORD = os.getenv('SN_PASSWORD')

assert SN_INSTANCE_NAME, 'SN_INSTANCE_NAME is not set'
assert SN_USERNAME, 'SN_USERNAME is not set'
assert SN_PASSWORD, 'SN_PASSWORD is not set'

SLEEP_BETWEEN_REQUESTS = 0.2

BASE_URL = f'https://{SN_INSTANCE_NAME}.service-now.com/api/now/table/'

def issue_sn_request(url):
  response = requests.get(url, auth=(SN_USERNAME, SN_PASSWORD))

  if response.status_code >= 400:
    print(f'\033[91m{response.status_code}\033[0m', end=' ')
  else:
    print(f'\033[92m{response.status_code}\033[0m', end=' ')

  print(url)

  return response.json()

# Get information about assets associated with a computer.
def get_computer_assets(computer_id, asset_type = 'cmdb_ci'):
  return issue_sn_request(
    f'{BASE_URL}{asset_type}?sysparm_query=computer={computer_id}'
  )

# Get information about a single asset.
def get_cmdb_info(asset_id, table_name = 'cmdb_ci', query = ''):
  return issue_sn_request(f'{BASE_URL}{table_name}/{asset_id}{query}')

# Get information about file system items
def get_file_system_items(computer_id):
  return issue_sn_request(
    f'{BASE_URL}cmdb_ci_file_system?sysparm_query=computer={computer_id}'
  )

# Get information about storage items
def get_storage_devices(computer_id):
  return issue_sn_request(
    f'{BASE_URL}cmdb_ci_storage_device?sysparm_query=computer={computer_id}'
  )

# Get information about FC disks
def get_fc_disks(computer_id):
  return issue_sn_request(
    f'{BASE_URL}cmdb_ci_fc_disk?sysparm_query=computer={computer_id}'
  )

# Get information about all children of a computer
def get_children(computer_id):
  return issue_sn_request(
    f'{BASE_URL}cmdb_rel_ci?sysparm_query=parent={computer_id}'
  )

# Write a JSON file to disk.
def write_json_file(data, filename):
  with open(filename, 'w') as file:
    json.dump(data, file, indent=2)
