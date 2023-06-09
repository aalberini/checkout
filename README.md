# Checkout

## Install dotnet-ef, for running scripts to create SQLite database, from command line:
dotnet tool install --global dotnet-ef

## To execute database creation and generate migrations, from command line:
dotnet ef migrations add FirstMigration -o Infrastructure/Persistence/Migrations

## To create or update database structure, from command line:
dotnet ef database update

## To remove migrations, from command line:
dotnet ef migrations remove

Before to start, you must add some products to database using the endpoint provided.
