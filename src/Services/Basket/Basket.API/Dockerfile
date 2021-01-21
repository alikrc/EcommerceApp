#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "EcommerceApp.sln" "EcommerceApp.sln"

COPY "EventBus/EventBus/EventBus.csproj" "EventBus/EventBus/EventBus.csproj"
COPY "EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "Services/Basket/Basket.API/Basket.API.csproj" "Services/Basket/Basket.API/Basket.API.csproj"
COPY "Services/Identity/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/Identity.API.csproj"
COPY "docker-compose.dcproj" "docker-compose.dcproj"

RUN dotnet restore "EcommerceApp.sln"

COPY . .
WORKDIR "/src/Services/Basket/Basket.API"
RUN dotnet publish --no-restore -c Release -o /app

FROM build as unittest
WORKDIR /src/Services/Basket/Basket.UnitTests

FROM build as functionaltest
WORKDIR /src/Services/Basket/Basket.FunctionalTests

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.API.dll"]