# https://hub.docker.com/r/jboss/keycloak/
FROM keycloak/keycloak:24.0.1

EXPOSE 8080

COPY ./config/*.json /tmp/

ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start-dev", "--import-realm"]
