## Migrations


****Auth****

```dotnet ef migrations add <name> --context AuthContext -o Migrations/Auth```

```dotnet ef database update --context AuthContext```



****Crm****

```dotnet ef migrations add <name> --context CrmContext -o Migrations/Crm```

```dotnet ef database update --context CrmContext```
