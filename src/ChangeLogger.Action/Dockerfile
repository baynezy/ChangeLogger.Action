FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish ./src/ChangeLogger.Action/ChangeLogger.Action.csproj -c Release -o out --no-self-contained

FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine

COPY --from=build-env /app/out .

USER root

# 1001 is the default user in GitHub Actions runners
RUN chown -R 1001:1001 /github/workspace || true
USER 1001

ENTRYPOINT [ "dotnet", "/ChangeLogger.Action.dll" ]