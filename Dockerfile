# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./API /src/API
COPY ./Shared /src/Shared
RUN dotnet restore "API/API.csproj"
WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

RUN adduser --uid 10001 --disabled-password --gecos "" nonroot

WORKDIR /app
COPY --from=build /app/publish .
USER 10001
ENTRYPOINT ["dotnet", "At.luki0606.DartZone.API.dll"]