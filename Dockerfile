# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY EstroAPI/*.csproj ./EstroAPI/
RUN dotnet restore

COPY EstroAPI/. ./EstroAPI/
WORKDIR /src/EstroAPI
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EstroAPI.dll"]
