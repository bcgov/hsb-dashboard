# Iterate through all ./children/ok and find all the sys_class_name fields. Each
# file in the directory is a JSON file, and the result is at the `result` key.

import os
import json

# Load env variables
from dotenv import load_dotenv

HIGHLIGHT_SYS_CLASS_NAMES = [
  'cmdb_ci_fc_disk', 'cmdb_ci_nas_file_system'
]

# Iterate through all folders in the directory. First, start with a listing of
# all subdirectories in the current directory.
for folder in os.listdir('downloads'):

  print(folder)

  print(os.path.isdir(os.path.join('downloads', folder)))

  # If the folder is not a directory, skip it
  if not os.path.isdir(os.path.join('downloads', folder)):
    continue

  folder_children_ok_name = f'downloads/{folder}/children/ok'

  print(folder_children_ok_name)

  # If the folder does not have a children/ok subdirectory, skip it
  if not os.path.exists(folder_children_ok_name):
    continue

  # Path to the directory
  directory = f'downloads/{folder}/children/ok'

  # Get the list of files in the directory
  files = os.listdir(directory)

  # Print the folder name with space around it and some hashes to pad it to
  # 80 characters. Also print how many children are in the folder in brackets.
  print()
  print(f'{"#" * 40} {folder} ({len(files)} children) {"#" * 40}')

  # Get an array of the sys_class_name fields. We are going to sort it later.
  # Each item in the array should have a few fields, populated as follows:
  #  - sys_class_name: The sys_class_name field from the JSON file
  #  - name: The name of the object
  #  - sys_id: The sys_id of the object
  items = []

  # Iterate through the files
  for file in files:
    # Load the JSON data
    with open(f'{directory}/{file}', 'r') as file:
      data = json.load(file)

    # Add the item to the items array, taking the key-value pairs we want
    items.append({
      'sys_class_name': data['result']['sys_class_name'],
      'name': data['result']['name'],
      'sys_id': data['result']['sys_id']
    })

  # Sort the items array by the sys_class_name field, then the name, and print
  # them. Print the sys_id, then the sys_class_name, then the name. Print the
  # items in columnar format. If the sys_class_name of the file is in the
  # HIGHLIGHT_SYS_CLASS_NAMES array, print the sys_class_name in green.
  items.sort(key=lambda x: (x['sys_class_name'], x['name']))
  for item in items:
    if item['sys_class_name'] in HIGHLIGHT_SYS_CLASS_NAMES:
      print(f'\033[92m{item["sys_id"]:<40} {item["sys_class_name"]:<32} {item["name"]}\033[0m')
    else:
      print(f'{item["sys_id"]:<40} {item["sys_class_name"]:<32} {item["name"]}')
