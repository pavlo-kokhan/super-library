FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Library/Library.Api/Library.Api.csproj", "Services/Library/Library.Api/"]
COPY ["Services/Library/Library.Domain/Library.Domain.csproj", "Services/Library/Library.Domain/"]
COPY ["Shared/SuperLibrary.DomainAbstractions/SuperLibrary.DomainAbstractions.csproj", "Shared/SuperLibrary.DomainAbstractions/"]
COPY ["Shared/SuperLibrary.Web/SuperLibrary.Web.csproj", "Shared/SuperLibrary.Web/"]
COPY ["Services/Library/Library.Infrastructure/Library.Infrastructure.csproj", "Services/Library/Library.Infrastructure/"]
RUN dotnet restore "Services/Library/Library.Api/Library.Api.csproj"
COPY . .
WORKDIR "/src/Services/Library/Library.Api"
RUN dotnet build "./Library.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Library.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library.Api.dll"]
