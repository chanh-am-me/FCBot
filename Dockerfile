# Use the official .NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
# Use the official .NET Core SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FCBot.sln", "./"]
COPY ["src/Web/Web.csproj", "src/src/Web/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/src/Infrastructure/"]
COPY ["src/Migration.Postgres/Migration.Postgres.csproj", "src/src/Migration.Postgres/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/"
RUN dotnet build "Web.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish
# Build the final image using the base image and the published output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]