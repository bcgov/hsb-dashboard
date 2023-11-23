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
| .NET SDK 8.0                                                              | [download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) |

Execute the following command to initialize your local environment and start the required Docker containers.

> If you run into errors with a Mac it is most likely a script needs execute permission.

```bash
# Start the database and run the migration.
# Spin up all other required containers.
bash do init
```

The default configuration will initialize the database and run the web application.

```bash
# Open the web application in your default browser
bash do go
```
