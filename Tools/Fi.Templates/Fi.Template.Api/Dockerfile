FROM fimple.azurecr.io/fi-docker/dotnet/fi-runtime-base:7.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY ["Src/Fi.TemplateUniqueName.Api/Fi.TemplateUniqueName.Api.csproj", "Src/Fi.TemplateUniqueName.Api/"]
COPY ["Src/Fi.TemplateUniqueName.Schema/Fi.TemplateUniqueName.Schema.csproj", "Src/Fi.TemplateUniqueName.Schema/"]

RUN dotnet restore "Src/Fi.TemplateUniqueName.Api/Fi.TemplateUniqueName.Api.csproj"  

COPY ["Src/", "Src/"]

RUN dotnet publish "Src/Fi.TemplateUniqueName.Api/Fi.TemplateUniqueName.Api.csproj" -c Release \
  -o /app/publish

FROM base AS final
COPY --from=build --chown=app:fimple /app/publish .

ENTRYPOINT ["dotnet", "Fi.TemplateUniqueName.Api.dll"]