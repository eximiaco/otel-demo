FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Inscricoes/OtelDemo.Inscricoes.BrokerConsumer/OtelDemo.Inscricoes.BrokerConsumer.csproj", "Inscricoes/OtelDemo.Inscricoes.BrokerConsumer/"]
RUN dotnet restore "Inscricoes/OtelDemo.Inscricoes.BrokerConsumer/OtelDemo.Inscricoes.BrokerConsumer.csproj"
COPY . .
WORKDIR "/src/Inscricoes/OtelDemo.Inscricoes.BrokerConsumer"
RUN dotnet build "OtelDemo.Inscricoes.BrokerConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OtelDemo.Inscricoes.BrokerConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtelDemo.Inscricoes.BrokerConsumer.dll"]
