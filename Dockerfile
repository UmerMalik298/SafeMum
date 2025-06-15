# Base stage for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#ENV DOTNET_RUNNING_IN_CONTAINER=true


# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first for better layer caching
COPY ["SafeMum.API/SafeMum.API.csproj", "SafeMum.API/"]
COPY ["SafeMum.Application/SafeMum.Application.csproj", "SafeMum.Application/"]
COPY ["SafeMum.Domain/SafeMum.Domain.csproj", "SafeMum.Domain/"]
COPY ["SafeMum.Infrastructure/SafeMum.Infrastructure.csproj", "SafeMum.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "SafeMum.API/SafeMum.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/SafeMum.API"
RUN dotnet build "SafeMum.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SafeMum.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for production
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SafeMum.API.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet SafeMum.API.dll