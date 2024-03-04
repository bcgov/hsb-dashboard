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

## Database Migrations

Database migrations are built with Entity Framework. Dotnet tooling provides a Code-First approach to database migration, which enables the generation of migrations that apply new versions and perform rollbacks to prior versions. These tools provide a simple repeatable and testable Infrastructure as Code implementation.

To create a new database migration it is as simple as executing the following command `bash do db-add {version}`. This will generate a migration that will apply any changes you have made to the database entities and structure, and also includes a way to run SQL scripts. Scripts must be placed in appropriate folders which are convention based `./src/libs/dal/Migrations/{version}/Up/PreUp`, `./src/libs/dal/Migrations/{version}/Up/PostUp`, `./src/libs/dal/Migrations/{version}/Down/PreDown`, `./src/libs/dal/Migrations/{version}/Down/PostDown`. It is also possible to execute scripts at any point in the migration through code.

To run the migration execute the following command `bash do db-migration`. This will build an image and run a container to perform the database migration. If you have the Dotnet SDK and Entity Framework tools installed locally you can execute the following command instead `bash do db-update`, this is more performant for machines with little RAM.

There are a few other helpful database migration commands that can help with development.

| Command      | Description                                                 |
| ------------ | ----------------------------------------------------------- |
| db-list      | Displays all migration version available                    |
| db-add       | Generates a new migration                                   |
| db-migration | Builds and run container to perform migration               |
| db-update    | Builds and runs migration locally                           |
| db-rollback  | Roles back to the specified version                         |
| db-remove    | Removes the files associated with the most recent migration |
| db-drop      | Drops the database                                          |
| db-refresh   | Drops the database and runs all the migrations              |
| db-redo      | Rollback and reapply the migration                          |

## Script Help

There are several scripts and commands to help local development and the CI/CD pipeline.

```bash
# Show all the help commands
bash do help
```

The Dashboard web application is setup for hot-reload within a Docker container.

### CI/CD Pipelines

When a Pull Request is created Github Actions will build and test the images to provide feedback on issues.

When a Pull Request is merged into the `main` branch Github Actions will build and publish the images to the default Github Packages image registry. There are four packages built for this project.

Artifactory which is hosted by the Exchange Lab will pull in these packages every 15 minutes. All of these images are tagged with `latest` (all attempts to also tag them with other values have not been successful for some reason).

There are Tekton pipelines created to assist in deployments of the `db-migration` and `data-service`. Note that the `data-service` takes more than an hour to run, as such you will need to create a PipelineRun object (example is in source) to configure a longer run time. These pipelines provide a simple parameter based way to run theses containers in the appropriate environment.

#### Options

| Argument  | Values                  | Required | Default |
| --------- | ----------------------- | -------- | ------- |
| component | api,db,app,data-service | \*       |         |
| tag       |                         |          | latest  |
| env       | dev, test, prod         |          | dev     |

### Build Images

If you would like to build an image locally and push it to Openshift you can use the following commands. Replace the curly brackets with an appropriate value.

`bash do oc-build {component} {tag=latest}`

### Push Images

To push a local image to Openshift registry use the following command.

`bash do oc-push {component}`

### Deploy Images

To deploy an image to an environment in Openshift use the following command. This makes the images available to those environments and they will be automatically deployed.

`bash do oc-deploy {component} {tag=latest} {env=dev}`

### Run a Container Remotely

There are two components that are run as part of backend services, or deployments. The first is the `db-migration` which applies database migrations. The second is the `data-service` which syncs data with Service Now.

Run the database migration.

`bash do oc-run db {tag=latest} {env=dev}`

Run the Data Service.

`bash do oc-run data-service {env=dev}`
