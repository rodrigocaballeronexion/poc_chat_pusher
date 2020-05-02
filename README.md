# Preparing env
For migrations
dotnet tool install --global dotnet-ef --version 3.1.3
dotnet tool update --global dotnet-ef


# EF Core
dotnet-ef migrations add init

dotnet-ef database update