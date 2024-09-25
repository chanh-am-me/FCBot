# Use the official .NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app
# Use the official .NET Core SDK as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Web.csproj", "./"]
RUN dotnet restore "./src/Web.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "src/Web.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "src/Web.csproj" -c Release -o /app/publish
# Build the final image using the base image and the published output
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "src/Web.dll"]