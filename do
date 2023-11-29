#!/bin/bash

. ./scripts/variables.sh
. ./scripts/do-args.sh
. ./scripts/docker.sh
. ./scripts/setup.sh
. ./scripts/help.sh
. ./scripts/database.sh
. ./scripts/oc.sh

action=${1-"help"}

if [ "$action" = "help" ]; then
  do_help
elif [ "$action" = "setup" ]; then
  gen_env ${2-}
elif [ "$action" = "init" ]; then
  s='' # Clear service argument
  gen_env ${2-}
  docker_up database
  db-migration
  docker_up

# Docker
elif [ "$action" = "up" ]; then
  docker_up
elif [ "$action" = "build" ]; then
  docker_build
elif [ "$action" = "stop" ]; then
  docker_stop
elif [ "$action" = "down" ]; then
  docker_down
elif [ "$action" = "refresh" ]; then
  docker_refresh $s
elif [ "$action" = "remove" ]; then
  docker_remove $s
elif [ "$action" = "nuke" ]; then
  docker_nuke
elif [ "$action" = "ssh" ]; then
  docker exec -it "hsb-$2" sh

# Database
elif [ "$action" = "tool-update" ]; then
  tool-update $s
elif [ "$action" = "db-migration" ]; then
  db-migration $s
elif [ "$action" = "db-migrations" ]; then
  db-migrations $s
elif [ "$action" = "db-add" ]; then
  db-add $s
elif [ "$action" = "db-update" ]; then
  db-update $s
elif [ "$action" = "db-rollback" ]; then
  db-rollback $s
elif [ "$action" = "db-remove" ]; then
  db-remove $s
elif [ "$action" = "db-drop" ]; then
  db-drop $s
elif [ "$action" = "db-refresh" ]; then
  db-refresh $s
elif [ "$action" = "db-redo" ]; then
  db-redo $s

# Openshift
elif [ "$action" = "db-connect" ]; then
  db-connect ${2-prod} ${3-} ${4-}
elif [ "$action" = "deploy" ]; then
  deploy ${2-} ${3-} ${4-}

# Other
elif [ "$action" = "go" ]; then
  if [[ "$OSTYPE" == "darwin"* ]]; then # Macos
    open http://localhost:30080
  elif [[ "$OSTYPE" == "win32" ]] || [[ "$OSTYPE" == "msys" ]]; then # Windows
    start http://localhost:30080
  else
    xdg-open http://localhost:30080
  fi
else
  echo "Invalid action '$action', refer to help."
fi
