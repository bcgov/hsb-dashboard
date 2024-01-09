FROM postgres:16.1

RUN groupadd non-root-postgres-group
RUN useradd admin --group non-root-postgres-group
RUN chown -R admin:non-root-postgres-group /var/lib/postgresql/data
RUN chmod 777 /var/lib/postgresql/data
RUN chmod 777 /var/run/postgresql
USER admin

COPY ./scripts /docker-entrypoint-initdb.d/
