FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RinhaBackend.Api/RinhaBackend.Api.csproj", "RinhaBackend.Api/"]
COPY ["RinhaBackend.Services/RinhaBackend.Services.csproj", "RinhaBackend.Services/"]
COPY ["RinhaBackend.Infrastructure/RinhaBackend.Infrastructure.csproj", "RinhaBackend.Infrastructure/"]
COPY ["RinhaBackend.Domain/RinhaBackend.Domain.csproj", "RinhaBackend.Domain/"]
RUN dotnet restore "RinhaBackend.Api/RinhaBackend.Api.csproj"
COPY . .
WORKDIR "/src/RinhaBackend.Api"
RUN dotnet build "RinhaBackend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RinhaBackend.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "RinhaBackend.Api.dll"]
