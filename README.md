You need to creeate database via Packet Manager (Update-Database) or Terminal (dotnet ef database update) for both contexts: IdentityContext and ApplicationDbContext
Or you can use CreateDb.bat for that (Windows only)
