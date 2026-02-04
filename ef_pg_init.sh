dotnet tool install --global dotnet-ef --version 10.*
docker run -d --name nimbo-postgres -e POSTGRES_USER=nimbo_admin -e POSTGRES_PASSWORD=nimbo_password -e POSTGRES_DB=nimbo_wms -p 5432:5432 -v nimbo_pg_data:/var/lib/postgresql/data postgres:16
