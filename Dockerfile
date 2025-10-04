# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore
COPY EbayChat.csproj ./
RUN dotnet restore EbayChat.csproj

# copy all source and build
COPY . ./

# install EF tools (only in build stage)
RUN dotnet tool install --global dotnet-ef --version 9.*
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EbayChat.dll"]

# --- Optional Migration Stage (for docker-compose) ---
FROM build AS migrate
WORKDIR /src
ENTRYPOINT ["dotnet", "ef", "database", "update"]
