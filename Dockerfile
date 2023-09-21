FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY eagles-food-backend/*.csproj .
RUN dotnet restore --use-current-runtime

# copy everything else and build app
COPY eagles-food-backend/* .
RUN dotnet publish -c release -o /app --sc false --use-current-runtime --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "eagles-food-backend.dll"]