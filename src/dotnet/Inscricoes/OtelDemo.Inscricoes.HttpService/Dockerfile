FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Inscricoes/OtelDemo.Inscricoes.HttpService/OtelDemo.Inscricoes.HttpService.csproj", "Inscricoes/OtelDemo.Inscricoes.HttpService/"]
RUN dotnet restore "Inscricoes/OtelDemo.Inscricoes.HttpService/OtelDemo.Inscricoes.HttpService.csproj"
COPY . .
WORKDIR "/src/Inscricoes/OtelDemo.Inscricoes.HttpService"
RUN dotnet build "OtelDemo.Inscricoes.HttpService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OtelDemo.Inscricoes.HttpService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtelDemo.Inscricoes.HttpService.dll"]
