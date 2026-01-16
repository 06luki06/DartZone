# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props .
COPY Directory.Packages.props .
COPY .editorconfig .

COPY ./API /src/API
COPY ./Shared /src/Shared

RUN dotnet restore "API/API.csproj"

WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .
USER app
ENTRYPOINT ["dotnet", "At.luki0606.DartZone.API.dll"]