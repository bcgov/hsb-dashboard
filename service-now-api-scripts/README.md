# Service Now API Scripts

Scripts to help with discovery and exploration of the raw Service Now data. This is help for quick exploration of servers, without having to manually write API requests in Postman, or waiting for the Data Service to run.

## Prerequisites

- Python 3.6 or higher
- Python `requests` library
- Service Now API credentials and instance URL

## Motivation

The Data Service provides a lot of useful information about servers, but it can be slow to run, and it can be difficult to explore the data in a way that is useful for discovery.

For instance, sometimes we want to grab all the file system items associated with a particular server.

These scripts were written to help developers with that.

## Setup

Create a `.env` file in the root of the project with the following credentials. They can be obtained from the `.env` file of the Data Service.

```bash
SN_INSTANCE_NAME=servicenow-instance-name
SN_USERNAME=service-now-username
SN_PASSWORD=service-now-password
COMPUTER_IDS=abc123,def456,ghi789
```

Note that the `SN_INSTANCE_NAME` is not the URL of the deployment, but the name of the instance. For example, if the URL is `https://dev12345.service-now.com`, the instance name is `dev12345`.

The `COMPUTER_IDS` is a comma-separated list of computer IDs that you want to query. (These are the `sys_id` values of the computers in Service Now.) They can be obtained by inspecting the `ServerItem` table in the HSB database, and looking at values in the `ServiceNowKey` column.

## Usage

To run the scripts, execute the following command:

```bash
python3 scriptname.py
```

More information on the scripts is below.

### `get_basic_info.py`

This script will get basic information about the computers in Service Now. It will create a folder in the `./downloads` folder with the name of the computer, and save various information in .json files in that folder:

- `computer_info.json`: Basic information about the computer
- `file_system_items.json`: Information about the file system items associated with the computer
- `storage_devices.json`: Information about the storage devices associated with the computer
- `fc_disks.json`: Information about the fibre channel (SAN) disks associated with the computer
- `children.json`: All the children of the computer: i.e. Service Now items from the `cmdb_rel_ci` table that have this computer listed as their parent

The script will also attempt to download all children of the server, and save them in a subfolder: `children/ok` for children which can be downloaded, and `children/error` for children which cannot be downloaded (typically due to access restrictions).

This process might take some time. The script will sleep 0.5 seconds between each request to avoid hitting the API rate limit.

Also, note that **the script will overwrite the .json files listed above, except for any children that have been downloaded**. This is to ensure that the most recent data is always available. To re-download children, delete the `children` folder.

### `print_sys_class_names.py`

This script will print out all the `sys_class_name` values for the "ok" children of the computers in the `downloads` folder. This is useful for exploring all the Service Now items associated with the computer.

### `print_disks.py`

This script takes a single argument: the computer name of the computer you want to query. It will print out all the children of the computer (obtained in the previous step) for you to review.

So, for instance:

```bash
python3 print_disks.py my-computer-name
```
