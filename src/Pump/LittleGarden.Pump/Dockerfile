FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Pump/LittleGarden.Pump/LittleGarden.Pump.csproj", "src/Pump/LittleGarden.Pump/"]
RUN dotnet restore "src/Pump/LittleGarden.Pump/LittleGarden.Pump.csproj"
COPY . .
WORKDIR "/src/src/Pump/LittleGarden.Pump"
RUN dotnet build "LittleGarden.Pump.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LittleGarden.Pump.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LittleGarden.Pump.dll"]
