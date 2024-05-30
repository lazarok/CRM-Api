## Migrations

**Development**

***you need to reference Microsoft.EntityFrameworkCore.Design***

```export ASPNETCORE_ENVIRONMENT=local```


****Auth****

```dotnet ef migrations add <name> --context AuthContext -o Migrations/Auth```

```dotnet ef database update --context AuthContext```




****Crm****

```dotnet ef migrations add <name> --context CrmContext -o Migrations/Crm```

```dotnet ef database update --context CrmContext```