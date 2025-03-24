# OCIO HSB Dashboard

The OCIO HSB Dashboard is a web application that provides a visualization of storage allocation and consumption for the Office of the Chief Information Officer (OCIO) Hosting Services Branch (HSB).

## Project Background

The Office of the Chief Information Officer’s Enterprise Services Division enables Government to deliver services to citizens by providing high quality, secure and cost-effective information management, and technology services. In Enterprise Services Division, Hosting Services enables B.C. government and broader public sector (BPS) to access sustainable hosting infrastructure and services to support the evolving needs of government clients and citizens.

The OCIO’s storage infrastructure offers redundancy, configuration flexibility, support services and management software capabilities for supporting different data types, applications, and processing platforms. Client ministries can choose one or more of three tiers of storage, in 50 GB increments, that align with government business data requirements.

This dashboard aims to allow users to visualize their storage allocation and consumption through different views and perspectives to help them understand their current consumption. It will provide dynamic access to server storage consumption and allocation data for client use and HSB use. The goal is to allow users to make data-backed decisions on how to best reduce or optimize their storage utilization.

## High-Level Architecture

### Components

The project consists of several components. The main components are:

- An **API** (`api`) that provides the backend services for both the HSB front-end and the Data Service.
- The **HSB dashboard front-end** (variously called the `app` or `dashboard`), which is a Next.js project.
- A **Data Service** (`data-service`) that syncs data with Service Now. It is a C# project which is set up to run once a day and get all the information the project needs from Service Now. It downloads all the data and reconciles it with the HSB database
- The **database** (`db`) that stores the application data, including user and organzation information, information on current and historical Server Items and File System Items.

The `/src` folder in the project root has most of the project code that a developer would typically edit:

- `/src/api` contains the C# project for the API.
- `/src/api-css` connects to the Common Hosted Single Sign-On (CSS) — see below for more detail.
- `/src/dashboard` contains the front end dashboard.
- `/src/data-service` contains the C# project for the Data Service.
- `/src/libs` contains library files that are reused throughout the C# code. These files generally do not need to be edited.

The `api`, `api-css`, `dashboard`, and `data-service` components are Dockerized and will each run in their own Docker container locally and remotely on OpenShift.

### Deployments

The application has 3 deployments on OpenShift, in the BC Government's EMERALD Cluster: `dev`, `test`, and `prod`.

This guide assumes a basic familiarity with OpenShift concepts.

### User Authentication & Authorization

The application uses a Keycloak integration for user authentication and authorization.

Though the application's database keeps track of user, group, and role data locally, the single source of truth of this data is Keycloak. If a user has a permission in the HSB app database but not on Keycloak, they will not be able to access that application feature.

The application has an entry on the [Common Hosted Single Sign-on (CSS) console](https://bcgov.github.io/sso-requests) through which you can obtain and validate Keycloak settings.

There are 3 CSS environments corresponding to the 3 deployment levels: `dev`, `test`, and `prod`.

## Local Development

### Bash Script

The project contains a useful bash script, `bash do`, which will help with a number of development tasks and will be referred to throughout the following docs.

```bash
# Show all the help commands
bash do help
```

### Get Started Developing

#### Prerequisites

You'll need to install the following.

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

##### Mac Requirements

You will need to install `coreutils`, `gnu-getopt`, and `gsed` using [Homebrew](https://brew.sh):

> brew install coreutils gnu-getopt gsed

If you run into other errors during the steps below, it is most likely a script needs execute permission.

#### Container Initialization

Execute the following command to initialize your local environment.

```bash
# Generate a local Keycloak admin username and password.
# Generate .env files.
# Start the database and run the migration.
# Spin up all other required containers.
#
bash do init
```

The script will initialize the database and various Docker containers.

#### Update .env Files

For the app to be fully functional, we will need to update the values of some `.env` file secrets.

##### Obtain the Keycloak Client Information

Although the setup script will set you up with a local Keycloak deployment, it is recommended to connect directly to the dev SSO environment even for local development. The appropriate settings below can be obtained by investigating the `keycloak` and `css` secrets and configmaps from the Emerald `dev` deployment on OpenShift.

##### Update API env File

In `/src/api/.env`:

Update these lines the appropriate secrets:

```bash
Keycloak__Authority=https://dev.loginproxy.gov.bc.ca/auth/realms/standard
Keycloak__Audience=[obtain from dev configmap / secrets: starts with `hsb-`]
Keycloak__Issuer=[obtain from dev configmap / secrets: starts with `hsb-`]
Keycloak__Secret=[obtain from dev configmap / secrets: alphanumeric secret]
CSS__Environment=dev
CSS__ApiUrl=https://api.loginproxy.gov.bc.ca
CSS__Authority=https://loginproxy.gov.bc.ca
CSS__ClientId=[obtain from dev configmap / secrets: starts with `service-account-`]
CSS__Secret=[obtain from dev configmap / secrets: alphanumeric secret]
```

#### Update API-CSS env File

In `/src/api-css/.env`, update two lines with `hsb-app` and the appropriate client secret, respectively:

```bash
Keycloak__ClientId=hsb-app
Keycloak__Secret=[obtain from dev configmap / secrets: alphanumeric secret]
```

#### Update Dashboard env File

In `/src/dashboard/.env`:

Update the line `KEYCLOAK_SECRET={GET FROM KEYCLOAK}` with the appropriate secret:

```bash
KEYCLOAK_SECRET=[obtain from dev configmap / secrets: alphanumeric secret]
```

Note that you can also **uncomment** the following lines to skip Keycloak authentication altogether:

```bash
# NEXT_PUBLIC_AUTH_STATUS=authenticated
# NEXT_PUBLIC_AUTH_ROLES=hsb
```

This should only be done for development purposes when testing authentication is not necessary.

#### Restart Environment

The following command will rebuild the Docker containers to pick up all the `.env` file changes you've made above:

```bash
bash do up
```

#### Run the web application

Now we can start the web application:

```bash
# Open the web application in your default browser
bash do go
```

The Dashboard web application is setup for hot-reload within a Docker container.

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

If you get an error on a Mac with an M chip, try running:

```bash
dotnet tool uninstall dotnet-ef --global
dotnet tool install dotnet-ef --global -a arm64
```

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

## Other Helpful Documentation

- [API Swagger](https://localhost:30005/api-docs)
- [Docker Compose Cheat Sheet](https://devhints.io/docker-compose)
- [Docker CLI Cheat Sheet](https://dockerlabs.collabnix.com/docker/cheatsheet/)
- [Next.js](https://nextjs.org/docs)
- [Dotnet Cheat Sheets](https://cheatography.com/tag/dotnet/)
- [Dotnet Entity Framework Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
