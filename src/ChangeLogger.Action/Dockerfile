FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish ./src/ChangeLogger.Action/ChangeLogger.Action.csproj -c Release -o out --no-self-contained

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "/ChangeLogger.Action.dll" ]