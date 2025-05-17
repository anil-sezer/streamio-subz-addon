# docker build -t anilsezer/subz:1.0.0 -t anilsezer/subz:latest .
# docker push anilsezer/subz:1.0.0
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the entire solution and all projects at once
COPY . .

# Restore the main project (which will include dependencies)
RUN dotnet restore "Subz.sln"

# Build the solution
WORKDIR "/src"
RUN dotnet build "Subz.sln" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Publish the main project
RUN dotnet publish "Subz/Subz.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subz.dll"]