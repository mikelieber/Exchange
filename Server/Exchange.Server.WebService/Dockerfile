FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/Exchange.Server.WebService/Exchange.Server.WebService.csproj", "Server/Exchange.Server.WebService/"]
RUN dotnet restore "Server/Exchange.Server.WebService/Exchange.Server.WebService.csproj"
COPY . .
WORKDIR "/src/Server/Exchange.Server.WebService"
RUN dotnet build "Exchange.Server.WebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchange.Server.WebService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.Server.WebService.dll"]
