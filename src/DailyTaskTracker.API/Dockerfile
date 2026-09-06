FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY DailyTaskTracker.sln .
COPY src/DailyTaskTracker.Domain/DailyTaskTracker.Domain.csproj src/DailyTaskTracker.Domain/
COPY src/DailyTaskTracker.Application/DailyTaskTracker.Application.csproj src/DailyTaskTracker.Application/
COPY src/DailyTaskTracker.Infrastructure/DailyTaskTracker.Infrastructure.csproj src/DailyTaskTracker.Infrastructure/
COPY src/DailyTaskTracker.API/DailyTaskTracker.API.csproj src/DailyTaskTracker.API/
COPY tests/DailyTaskTracker.UnitTests/DailyTaskTracker.UnitTests.csproj tests/DailyTaskTracker.UnitTests/
COPY tests/DailyTaskTracker.IntegrationTests/DailyTaskTracker.IntegrationTests.csproj tests/DailyTaskTracker.IntegrationTests/

RUN dotnet restore

# Copy all source code and publish release build
COPY . .
RUN dotnet publish src/DailyTaskTracker.API/DailyTaskTracker.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "DailyTaskTracker.API.dll"]
