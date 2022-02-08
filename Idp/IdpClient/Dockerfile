FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IdpClient/IdpClient.csproj", "IdpClient/"]
RUN dotnet restore "IdpClient/IdpClient.csproj"
COPY . .
WORKDIR "/src/IdpClient"
RUN dotnet build "IdpClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdpClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdpClient.dll"]
