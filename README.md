# Checkout

- Install dotnet-ef, for running scripts to create SQLite database, from command line:

  <b>dotnet tool install --global dotnet-ef</b>


- To execute database creation and generate migrations, from command line:

  <b>dotnet ef migrations add FirstMigration -o Infrastructure/Persistence/Migrations</b>


- To create or update database structure, from command line:

  <b>dotnet ef database update</b>


- To remove migrations, from command line:

  <b>dotnet ef migrations remove</b>


- Execute this SQL to insert admin user and role when run for the first time:

<b>insert into main.Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
  values  ('eed63df0-fb13-4c9c-8e65-9f7eef61e314', 'administrator', 'ADMINISTRATOR', null, null, 0, 'AQAAAAEAACcQAAAAEBemw0Ih3FoRGAv92lEoTDOsKR8uSpK8/vfqwTIUl+DtYzhTTj1XdsMPmBG1B39YJA==', 'HLXSGWLUYMLILCRKXWEWRPK4CN5GNFES', '7d1d4057-b66d-4381-8247-d7e010e7160f', null, 0, 0, null, 1, 0),
  ('917198d6-1b04-4036-8450-b9ccd6a68f44', 'other_user', 'OTHER_USER', null, null, 0, 'AQAAAAEAACcQAAAAEBvbdIvSYJ4b4F/tLfvoDlsIZju7RPzIZzbnLMqsUa2jgX5FTGHo/Qe8XZ2P13Ipuw==', 'N4W6Q7SSMS2AKJDH322X53A75EOYHS3U', 'bd092d45-b9a0-448c-8069-4f43f0ab8696', null, 0, 0, null, 1, 0);

  
insert into Roles (Id, Name, NormalizedName, ConcurrencyStamp)
  values  ('28683930-9172-4930-8c75-f16f3b924597', 'Admin', 'ADMIN', '28058046-870c-45f5-b365-9ff888c50de3');


  insert into UserRoles (UserId, RoleId)
  values  ('eed63df0-fb13-4c9c-8e65-9f7eef61e314', '28683930-9172-4930-8c75-f16f3b924597');</b>


- Before to start, you must add some products to database using the endpoint provided.


- Admin User:

  User: administrator - Pass: Adm1n1strat0R.1234


- Coomon User:

  User: other_user - Pass: Adm1n1strat0R.1234