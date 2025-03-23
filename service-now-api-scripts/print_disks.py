import json

# Load the argument to the script. That will be the name of the directory.
import sys
COMPUTER_NAME = sys.argv[1]


# Print that we're showing storage devices first. Include column headers.
print(f"\n💾 STORAGE DEVICES for {COMPUTER_NAME}")
print(f"{'=' * 98}\n")
print(f"{'Type':<25} {'Name':<40} {'Size':<10} {'Type':<20}")
print(f"{'-' * 25} {'-' * 40} {'-' * 10} {'-' * 20}")

# Load the JSON data
with open(f'./downloads/{COMPUTER_NAME}/storage_devices.json', 'r') as file:
  data = json.load(file)

  # Sort by sys_class_name, then by name.
  data['result'].sort(key=lambda x: (x['sys_class_name'], x['name']))

  # Print them out one per line, with the most relevant information (name, size, type, etc). Put it in columnar format.
  for item in data['result']:
    print(f"{item['sys_class_name']:<25} {item['name']:<40} {item['size']:<10} {item['storage_type']:<20}")

# Do the same for file systems
print(f"\n📂 FILE SYSTEMS for {COMPUTER_NAME}")
print(f"{'=' * 98}\n")
print(f"{'Type':<25} {'Name':<40} {'Size':<10} {'Type':<20}")
print(f"{'-' * 25} {'-' * 40} {'-' * 10} {'-' * 20}")

# Load the JSON data
with open(f'./downloads/{COMPUTER_NAME}/file_system_items.json', 'r') as file:
  data = json.load(file)

  # Sort by sys_class_name, then by name.
  data['result'].sort(key=lambda x: (x['sys_class_name'], x['name']))

  # Print them out one per line, with the most relevant information (name, size, type, etc). Put it in columnar format.
  for item in data['result']:
    print(f"{item['sys_class_name']:<25} {item['name']:<40} {item['size']:<10} {item['type']:<20}")
