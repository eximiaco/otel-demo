FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OtelDemo.Financeiro.HttpService/OtelDemo.Financeiro.HttpService.csproj", "OtelDemo.Financeiro.HttpService/"]
RUN dotnet restore "OtelDemo.Financeiro.HttpService/OtelDemo.Financeiro.HttpService.csproj"
COPY . .
WORKDIR "/src/OtelDemo.Financeiro.HttpService"
RUN dotnet build "OtelDemo.Financeiro.HttpService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OtelDemo.Financeiro.HttpService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtelDemo.Financeiro.HttpService.dll"]
