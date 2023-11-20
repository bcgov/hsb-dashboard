do_help () {
  echo \
"Do the following commands
---------------------------------------------------------------------
Action      | Arguments         | Description
---------------------------------------------------------------------
help:                             Documentation
setup:        [create|refresh]    Create .env files
up:           [{service}|-s|-p]   Run docker container(s)
build:        [{service}|-s|-p]   Build docker image(s)
stop:         [{service}|-s|-p]   Stop docker container(s)
down:         [{service}|-s|-p]   Stop and delete docker container(s)
refresh:      [{service}|-s|-p]   Stop, build, and run docker container(s)
ssh:          [{service}]         SSH into docker container
go:                               Open a browser and view app

tool-update:                      Updates the dotnet tool (default ef)
db-list:                          Lists all migrations
db-add:        {name}             Create a migration
db-update:     [{name}]           Update to the latest or specified migration
db-rollback:   [{name}]           Rollback to the specified migration
db-remove:     [{name}]           Remove the specified migration files
db-drop:                          Drop the databse
db-refresh:                       Drop and refresh the database
---------------------------------------------------------------------"
}
