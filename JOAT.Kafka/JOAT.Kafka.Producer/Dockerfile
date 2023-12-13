FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JOAT.Kafka.Producer/JOAT.Kafka.RatingsProducer.csproj", "JOAT.Kafka.Producer/"]
COPY ["JOAT.IFPA.ApiClient/JOAT.IFPA.ApiClient.csproj", "JOAT.IFPA.ApiClient/"]
COPY ["JOAT.Kafka.Consumer/JOAT.Kafka.RatingsConsumer.csproj", "JOAT.Kafka.Consumer/"]
COPY ["JOAT.Data/JOAT.Data.csproj", "JOAT.Data/"]
COPY ["JOAT.Kafka.Domain/JOAT.Kafka.Domain.csproj", "JOAT.Kafka.Domain/"]
RUN dotnet restore "./JOAT.Kafka.Producer/./JOAT.Kafka.RatingsProducer.csproj"
COPY . .
WORKDIR "/src/JOAT.Kafka.Producer"
RUN dotnet build "./JOAT.Kafka.RatingsProducer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JOAT.Kafka.RatingsProducer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JOAT.Kafka.RatingsProducer.dll"]