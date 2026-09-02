FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Host/Host.csproj src/Host/
COPY src/Modules/SalesDashboard.Domain/SalesDashboard.Domain.csproj src/Modules/SalesDashboard.Domain/
COPY src/Modules/SalesDashboard.Api/SalesDashboard.Api.csproj src/Modules/SalesDashboard.Api/
COPY src/Modules/SalesDashboard.Application/SalesDashboard.Application.csproj src/Modules/SalesDashboard.Application/
COPY src/Modules/SalesDashboard.Infrastructure/SalesDashboard.Infrastructure.csproj src/Modules/SalesDashboard.Infrastructure/
RUN dotnet restore src/Host/Host.csproj

COPY src/ src/
RUN dotnet publish src/Host/Host.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Host.dll"]
