# Etapa base para a execução da aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de construção da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/BackendChallenge.Api/
#COPY ["/BackendChallenge.Api/BackendChallenge.Api.csproj", "BackendChallenge.Api/"]
#RUN dotnet restore "./BackendChallenge.Api/BackendChallenge.Api.csproj"
COPY *.csproj ./
RUN dotnet restore
COPY . .
WORKDIR "/src/BackendChallenge.Api"
RUN dotnet build "./BackendChallenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicação da aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BackendChallenge.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final para a execução da aplicação
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendChallenge.Api.dll"]
