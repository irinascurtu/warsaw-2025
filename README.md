# warsaw-2025
warsaw workshop

[Documentation link:](https://polite-bay-0b3852803.2.azurestaticapps.net/)


```
docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```


## COnnection string for sql server running in Docker
```
"DefaultConnection": "Server=localhost,1433;Database=ProductDB;User=sa;Password=YourStrong@Password;TrustServerCertificate=True;"
```
