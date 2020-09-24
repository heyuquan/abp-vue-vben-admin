FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . .
WORKDIR /src/Mk.DemoB/src/Mk.DemoB.HttpApi.Host
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -nowarn:cs1998,cs1572,cs1573 --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish -nowarn:cs1998,cs1572,cs1573 --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mk.DemoB.HttpApi.Host.dll"]
