FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# HIER IST DIE WICHTIGE ÄNDERUNG: Gib den Ordnernamen mit an!
COPY ["Survey.ApiGateway/Survey.ApiGateway.csproj", "Survey.ApiGateway/"]
RUN dotnet restore "Survey.ApiGateway/Survey.ApiGateway.csproj"

COPY . .
RUN dotnet publish "Survey.ApiGateway/Survey.ApiGateway.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p wwwroot/uploads

EXPOSE 7224
ENTRYPOINT ["dotnet", "Survey.ApiGateway.dll"]