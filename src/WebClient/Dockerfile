FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/WebClient/WebClient.csproj", "WebClient/"]
RUN dotnet restore "src/WebClient/WebClient.csproj"
COPY . .
WORKDIR "/src/WebClient"
RUN dotnet build "WebClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebClient.dll"]
