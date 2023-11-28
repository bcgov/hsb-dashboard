FROM postgres:16.1

RUN groupadd non-root-postgres-group
RUN useradd non-root-postgres-user --group non-root-postgres-group
RUN chown -R non-root-postgres-user:non-root-postgres-group /var/lib/postgresql/data
RUN chmod 777 /var/lib/postgresql/data
USER non-root-postgres

COPY ./scripts /docker-entrypoint-initdb.d/
