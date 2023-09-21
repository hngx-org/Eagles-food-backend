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

## creating migration script and models:

when the database schema is changed, `migrate.sql` will need to be regenerated:

1. Connect to the production db.

```sh
$ mysql --host <host> -u eagles-admin -p
```

2. Dump the schema with `mysqldump`:

```sh
$  mysqldump -h <host> -u eagles-admin -p --no-data -B free_lunch_db --single-transaction > PROD_free_lunch_db.sql
```

3. Spin up a MySQL docker container with whatever values you need, or just use your installed version of MySQL.

```sh
$ docker run --rm -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=secret -e MYSQL_DATABASE=free_lunch_db mysql:8.1
```

4. Run the script

```sh
$ mysql > source PROD_free_lunch_db.sql
```

5. Generate the models

```sh 
$ dotnet ef dbcontext scaffold "Server=localhost;Database=free_lunch_db;user=root;password=secret" Pomelo.EntityFrameworkCore.MySql --output-dir ProdModels
```
