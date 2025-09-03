# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Stakeholders/*.csproj Stakeholders/
COPY Stakeholders.sln ./
RUN dotnet restore

COPY . .
WORKDIR /src/Stakeholders
RUN dotnet publish -c Debug -o /app/publish

# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Stakeholders.dll"]
