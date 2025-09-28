# 1) packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef

# 2) scaffold (example)
dotnet ef dbcontext scaffold "Server=localhost;User Id=sa;Password=123;Database=CloneEbayDB;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities --context-dir Data --context CloneEbayDbContext --use-database-names --no-onconfiguring --force


# 3) build
dotnet build

# 4) add baseline migration
dotnet ef migrations add InitialBaseline --context CloneEbayDbContext

# 5) make migration Up/Down empty (edit file)

# 6) apply baseline (records it)
dotnet ef database update --context CloneEbayDbContext
