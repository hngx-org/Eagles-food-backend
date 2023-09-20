# eagles-food-backend

## working with locally

1. Install [Docker](https://docs.docker.com/get-docker/)

2. Clone the repo

```sh
$ git clone https://github.com/hngx-org/Eagles-food-backend/
$ cd Eagles-food-backend
```

3. Run with Docker Compose

```sh
$ docker compose up
```

## running in production

1. Use the `production.yaml` for Docker Compose (TODO)

## creating migration script:

when the database schema is changed, `migrate.sql` will need to be regenerated:

1. Spin up a MySQL docker container with the values from `appsettings.json` (you can skip this if MySQL is already installed and running on your machine)

```sh
$ docker run --rm -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=secret -e MYSQL_DATABASE=free_lunch_db mysql:8.1
```

2. Generate the idempotent migration file

```sh
$ cd eagles-food-backend # the nested project folder, not the repo one
$ dotnet ef migrations script --idempotent > ../migrate.sql
```

3. Remove the first 4 lines of the generated file, since .NET is dumb and leaves its build output in:

```sh
$ cd ..
$ cat ./migrate.sql # before removing
Build started...
Build succeeded.
info: Microsoft.EntityFrameworkCore.Infrastructure[10403]
      Entity Framework Core 6.0.22 initialized 'LunchDbContext' using provider 'Pomelo.EntityFrameworkCore.MySql:6.0.2' with options: ServerVersion 8.1.0-mysql 
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
...more...

$ vi ./migrate.sql

$ cat ./migrate.sql # after editing
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
...more...
```