FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IdpApi/IdpApi.csproj", "IdpApi/"]
RUN dotnet restore "IdpApi/IdpApi.csproj"
COPY . .
WORKDIR "/src/IdpApi"
RUN dotnet build "IdpApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdpApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdpApi.dll"]
