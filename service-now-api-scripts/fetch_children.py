import json
import os
from dotenv import load_dotenv
import requests
import time

# Load the SN_USERNAME and SN_PASSWORD from the .env file
load_dotenv()

SN_USERNAME = os.getenv('SN_USERNAME')
SN_PASSWORD = os.getenv('SN_PASSWORD')

# Assert all the environment variables are set
assert SN_USERNAME, 'SN_USERNAME is not set'
assert SN_PASSWORD, 'SN_PASSWORD is not set'

def fetch_children(folder_name):

  # Path to the JSON file
  json_file_path = f'{folder_name}/children.json'

  # Load the JSON data
  with open(json_file_path, 'r') as file:
    data = json.load(file)

  # Get the result, which is an array of objects which might have child.link sub-keys
  result = data['result']

  # Get the "child" links
  child_links = [item['child']['link'] for item in result if 'child' in item]

  # Make directories if they don't exist
  os.makedirs(f'{folder_name}/children/ok', exist_ok=True)
  os.makedirs(f'{folder_name}/children/error', exist_ok=True)

  # Print the list of "child" links, one line per link. Also, issue a GET
  # request to each link and print the response. Note that we're using Basic Auth
  # to authenticate the request, with the SN_USERNAME and SN_PASSWORD values.
  # Also, wait 100ms between each request to avoid hitting the rate limit.
  for link in child_links:

    # First, check if we already downloaded the JSON file. Remember we have to
    # check both the "ok" and "error" directories. If we have downloaded the file
    # already, then we don't need to download it again.
    filename = link.split('/')[-1]
    if os.path.exists(f'{folder_name}/children/ok/{filename}.json'):
      print(f'Exists: \033[92m200\033[0m {link}')
      continue
    if os.path.exists(f'{folder_name}/children/error/{filename}.json'):
      print(f'Exists: \033[91m404\033[0m {link}')
      continue

    # Sleep for 50ms
    time.sleep(0.05)

    # Issue the GET request
    response = requests.get(link, auth=(SN_USERNAME, SN_PASSWORD))

    # Print the response code in red if an error 4xx, and green if it's 2xx. We
    # do NOT want a newline because we are going to print the URL on the same
    # line.
    if response.status_code >= 400:
      print(f'\033[91m{response.status_code}\033[0m', end=' ')
    else:
      print(f'\033[92m{response.status_code}\033[0m', end=' ')

    # Then print the URL ON THE SAME LINE as the response code
    print(response.url)

    # Save the JSON response to disk. If the status code is good, put it in
    # the "children/ok" directory, otherwise put it in the "children/error"
    # directory. The filename should be the last part of the URL.
    filename = response.url.split('/')[-1]
    if response.status_code >= 400:
      with open(f'{folder_name}/children/error/{filename}.json', 'w') as file:
        json.dump(response.json(), file, indent=2)
    else:
      with open(f'{folder_name}/children/ok/{filename}.json', 'w') as file:
        json.dump(response.json(), file, indent=2)
