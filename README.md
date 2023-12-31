# OCIO HSB Dashboard

## Ministry of Citizens' Services

The Office of the Chief Information Officer’s Enterprise Services Division enables Government to deliver services to citizens by providing high quality, secure and cost-effective information management, and technology services. In Enterprise Services Division, Hosting Services enables B.C. government and broader public sector (BPS) to access sustainable hosting infrastructure and services to support the evolving needs of government clients and citizens.

The OCIO’s storage infrastructure offers redundancy, configuration flexibility, support services and management software capabilities for supporting different data types, applications, and processing platforms. Client ministries can choose one or more of three tiers of storage, in 50 GB increments, that align with government business data requirements.

This dashboard aims to allow users to visualize their storage allocation and consumption through different views and perspectives to help them understand their current consumption. It will provide dynamic access to server storage consumption and allocation data for client use and HSB use. The goal is to allow users to make data-backed decisions on how to best reduce or optimize their storage utilization.

## Get Started Developing

Currently you'll need to install the following.

| Dependency                                                                | Link                                                               |
| ------------------------------------------------------------------------- | ------------------------------------------------------------------ |
| Docker Desktop, or equivalent that supports `docker` and `docker-compose` | [download](https://www.docker.com/products/docker-desktop/)        |
| Windows Bash (if you're using Windows OS)                                 | [download](https://git-scm.com/download/win)                       |
| Node Version Manager - NVM                                                | [download](https://github.com/coreybutler/nvm-windows/releases)    |
| .NET SDK 8.0 (only if you want to run .NET locally)                       | [download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) |

Docker Desktop on Windows defaults to WSL mode. This may work, but has known issues. You may need to turn WSL off.

When you install Node Version Manager (nvm) on Windows you will need to open a new command window as an administrator.
Run `nvm install 20.8.1` to install the version of Node required for this project.
Once it is installed run `nvm use 20.8.w1`.
If it installed correctly you can run `node -v` and it will display the correct version.

Execute the following command to initialize your local environment and start the required Docker containers.

> If you run into errors with a Mac it is most likely a script needs execute permission.

```bash
# Generate .env files.
# Start the database and run the migration.
# Spin up all other required containers.
# This process will ask you to input usernames and passwords.
bash do init
```

The default configuration will initialize the database and run the web application.

```bash
# Open the web application in your default browser
bash do go
```

## Script Help

There are a number of scripts and commands to help local development and the CI/CD pipeline.

```bash
# Show all the help commands
bash do help
```

The Dashboard web application is setup for hot-reload within a Docker container.
