#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/BackendChallenge.MicroServices
#COPY ["BackendChallenge.MicroServices/BackendChallenge.MicroServices.csproj", "BackendChallenge.MicroServices/"]
#RUN dotnet restore "./BackendChallenge.MicroServices/BackendChallenge.MicroServices.csproj"
COPY *.csproj ./
RUN dotnet restore
COPY . .
WORKDIR "/src/BackendChallenge.MicroServices"
RUN dotnet build "./BackendChallenge.MicroServices.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BackendChallenge.MicroServices.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BackendChallenge.MicroServices.dll"]