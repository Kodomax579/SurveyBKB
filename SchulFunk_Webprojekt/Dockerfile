FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["SSchulFunk_Webprojekt.csproj", "SchulFunk_Webprojekt/"]
RUN dotnet restore "SchulFunk_Webprojekt.csproj"

COPY . .
RUN dotnet publish "SchulFunk_Webprojekt.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 7189
ENTRYPOINT ["dotnet", "SchulFunk_Webprojekt.dll"]