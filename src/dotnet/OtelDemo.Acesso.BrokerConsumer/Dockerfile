FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OtelDemo.Acesso.BrokerConsumer/OtelDemo.Acesso.BrokerConsumer.csproj", "OtelDemo.Acesso.BrokerConsumer/"]
RUN dotnet restore "OtelDemo.Acesso.BrokerConsumer/OtelDemo.Acesso.BrokerConsumer.csproj"
COPY . .
WORKDIR "/src/OtelDemo.Acesso.BrokerConsumer"
RUN dotnet build "OtelDemo.Acesso.BrokerConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OtelDemo.Acesso.BrokerConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtelDemo.Acesso.BrokerConsumer.dll"]
