FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY aspnetcore/ aspnetcore/
COPY framework-aspnetcore/ framework-aspnetcore/
COPY "./NuGet.Config" "/src/gateways/InternalGateway.Host/NuGet.Config"
WORKDIR /src/gateways/InternalGateway.Host

RUN dotnet restore --disable-parallel --configfile "./NuGet.Config" -nowarn:msb3202,nu1503
RUN dotnet build -nowarn:cs1998,cs1572,cs1573 --no-restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -nowarn:cs1998,cs1572,cs1573 --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InternalGateway.Host.dll"]
