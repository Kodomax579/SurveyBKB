# Build-Stage: Nutze das .NET 10 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Survey.ApiGateway.csproj", "./"]
RUN dotnet restore "Survey.ApiGateway.csproj"
COPY . .
RUN dotnet publish "Survey.ApiGateway.csproj" -c Release -o /app/publish

# Run-Stage: Nutze das .NET 10 Runtime-Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# Erstelle den Upload-Ordner im Container
RUN mkdir -p wwwroot/uploads

ENTRYPOINT ["dotnet", "Survey.ApiGateway.dll"]