FROM fimple.azurecr.io/fi-docker/dotnet/fi-runtime-base:7.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY ["Src/Fi.Patika.Api/Fi.Patika.Api.csproj", "Src/Fi.Patika.Api/"]
COPY ["Src/Fi.Patika.Schema/Fi.Patika.Schema.csproj", "Src/Fi.Patika.Schema/"]

RUN dotnet restore "Src/Fi.Patika.Api/Fi.Patika.Api.csproj"  

COPY ["Src/", "Src/"]

RUN dotnet publish "Src/Fi.Patika.Api/Fi.Patika.Api.csproj" -c Release \
  -o /app/publish

FROM base AS final
COPY --from=build --chown=app:fimple /app/publish .

ENTRYPOINT ["dotnet", "Fi.Patika.Api.dll"]