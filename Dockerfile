FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PublicHolidaysApi/PublicHolidaysApi.csproj", "PublicHolidaysApi/"]
COPY ["PublicHolidaysApi.Tests/PublicHolidaysApi.Tests.csproj", "PublicHolidaysApi.Tests/"]
RUN dotnet restore "PublicHolidaysApi/PublicHolidaysApi.csproj"
RUN dotnet restore "PublicHolidaysApi.Tests/PublicHolidaysApi.Tests.csproj"

COPY . .

WORKDIR "/src/PublicHolidaysApi"
RUN dotnet build "PublicHolidaysApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/PublicHolidaysApi.Tests"
RUN dotnet build "PublicHolidaysApi.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet test "PublicHolidaysApi.Tests.csproj" -c $BUILD_CONFIGURATION --no-restore --verbosity normal

FROM build AS publish
WORKDIR "/src/PublicHolidaysApi"
RUN dotnet publish "PublicHolidaysApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicHolidaysApi.dll"]
