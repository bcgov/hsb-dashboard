import json
import os
from dotenv import load_dotenv
import requests
import time
import sn_helpers
import fetch_children

# Load the SN_USERNAME and SN_PASSWORD from the .env file
load_dotenv()

SN_INSTANCE_NAME = os.getenv('SN_INSTANCE_NAME')
SN_USERNAME = os.getenv('SN_USERNAME')
SN_PASSWORD = os.getenv('SN_PASSWORD')
COMPUTER_IDS = os.getenv('COMPUTER_IDS').split(',')

# Assert all the environment variables are set
assert SN_INSTANCE_NAME, 'SN_INSTANCE_NAME is not set'
assert SN_USERNAME, 'SN_USERNAME is not set'
assert SN_PASSWORD, 'SN_PASSWORD is not set'

# Get only the unique computer IDs
COMPUTER_IDS = list(set(COMPUTER_IDS))

# Iterate through the computer IDS
for COMPUTER_ID in COMPUTER_IDS:

  # Get some basic information about the computer in question.
  info = sn_helpers.get_cmdb_info(COMPUTER_ID, 'cmdb_ci_computer')

  # Make a directory for the name of the computer, if it doesn't exist
  os.makedirs(f'./downloads/{info["result"]["name"]}', exist_ok=True)

  # Get the computer name.
  computer_name = info['result']['name']

  # Save the basic information.
  sn_helpers.write_json_file(info, f'./downloads/{computer_name}/computer_info.json')

  # Get the file system items for the computer, and save them
  sn_helpers.write_json_file(sn_helpers.get_file_system_items(COMPUTER_ID),
    f'./downloads/{computer_name}/file_system_items.json')

  # Get the storage devices for the computer, and save them
  sn_helpers.write_json_file(sn_helpers.get_storage_devices(COMPUTER_ID),
    f'./downloads/{computer_name}/storage_devices.json')

  # Get the fibre channel disks for the computer, and save them
  sn_helpers.write_json_file(sn_helpers.get_fc_disks(COMPUTER_ID), f'./downloads/{computer_name}/fc_disks.json')

  # Get the children of the computer, and save them
  sn_helpers.write_json_file(sn_helpers.get_children(COMPUTER_ID),
    f'./downloads/{computer_name}/children.json')

  # Get the ok / error children of the computer, and save them
  fetch_children.fetch_children(f'./downloads/{computer_name}')
