# eagles-food-backend

Live link is at [https://hngxeaglesfood-4graz0d2.b4a.run/](https://hngxeaglesfood-4graz0d2.b4a.run/)

Swagger docs at [https://hngxeaglesfood-4graz0d2.b4a.run/swagger/index.html](https://hngxeaglesfood-4graz0d2.b4a.run/swagger/index.html)

Markdown docs at [DOCUMENTATION.md](eagles-food-backend/DOCUMENTATION.md)

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

1. Deploy the Dockerfile and set the following environment variables:

```sh
MYSQLCONNSTR_DefaultConnection = Server=3<host>;Database=<db>;user=<uname>;password=<pwd>
JWTSettings__SecretKey = <secret-that-is-at-least-512-bytes>
```

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

4. Copy the schema dump into the container

```sh
$ docker cp PROD_free_lunch_db.sql <container-id>:/PROD_free_lunch_db.sql
```

5. Log into MySQL and run the script

```sh
$ mysql > source PROD_free_lunch_db.sql
```

6. Modify your `appsettings.Development.json` to point to the new database

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=free_lunch_db;user=root;password=secret"
  }
}
```

7. Generate the models

```sh 
$ dotnet ef dbcontext scaffold "Server=localhost;Database=free_lunch_db;user=root;password=secret" Pomelo.EntityFrameworkCore.MySql --output-dir ProdModels
```

8. Manually copy the models into the `Models` directory and delete the `ProdModels` directory.
