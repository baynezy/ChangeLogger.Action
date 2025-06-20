FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

# Copy everything and publish the release (publish implicitly restores and builds)
WORKDIR /app
COPY . ./
RUN dotnet publish ./src/ChangeLogger.Action/ChangeLogger.Action.csproj -c Release -o out --no-self-contained

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine

# Set a consistent user for the runtime image
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

COPY --from=build-env /app/out .

# IMPORTANT: Ensure the 'appuser' owns the /github/workspace directory
# This step is critical for GitHub Actions scenarios where your container
# needs to run Git commands on the mounted repository.
# We do this as root, then switch to the non-root user.
# The `/github/workspace` directory is only available *at runtime* in GitHub Actions,
# so this chown needs to happen *before* the ENTRYPOINT or any Git commands.
USER root
RUN chown -R appuser:appgroup /github/workspace || true
USER appuser

ENTRYPOINT [ "dotnet", "/ChangeLogger.Action.dll" ]