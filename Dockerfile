
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:$PORT 
ENV DOTNET_RUNNING_IN_CONTAINER=true

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SafeMum.API/SafeMum.API.csproj", "SafeMum.API/"]
COPY ["SafeMum.Application/SafeMum.Application.csproj", "SafeMum.Application/"]
COPY ["SafeMum.Domain/SafeMum.Domain.csproj", "SafeMum.Domain/"]
COPY ["SafeMum.Infrastructure/SafeMum.Infrastructure.csproj", "SafeMum.Infrastructure/"]
RUN dotnet restore "./SafeMum.API/SafeMum.API.csproj"
COPY . .
WORKDIR "/src/SafeMum.API"
RUN dotnet build "./SafeMum.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SafeMum.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENV ASPNETCORE_URLS=http://+:$PORT
CMD ["dotnet", "SafeMum.API.dll"]

#ENTRYPOINT ["dotnet", "SafeMum.API.dll"]