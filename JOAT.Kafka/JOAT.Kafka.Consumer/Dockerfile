FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Install clang/zlib1g-dev dependencies for publishing to native
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JOAT.Kafka.Consumer/JOAT.Kafka.RatingsConsumer.csproj", "JOAT.Kafka.Consumer/"]
COPY ["JOAT.Data/JOAT.Data.csproj", "JOAT.Data/"]
COPY ["JOAT.IFPA.ApiClient/JOAT.IFPA.ApiClient.csproj", "JOAT.IFPA.ApiClient/"]
COPY ["JOAT.Kafka.Domain/JOAT.Kafka.Domain.csproj", "JOAT.Kafka.Domain/"]
RUN dotnet restore "./JOAT.Kafka.Consumer/./JOAT.Kafka.RatingsConsumer.csproj"
COPY . .
WORKDIR "/src/JOAT.Kafka.Consumer"
RUN dotnet build "./JOAT.Kafka.RatingsConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JOAT.Kafka.RatingsConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./JOAT.Kafka.RatingsConsumer"]