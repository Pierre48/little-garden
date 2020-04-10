FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Pump/LittleGarden.Pump/LittleGarden.Pump.csproj", "src/Pump/LittleGarden.Pump/"]
COPY ["src/Pump/Pumps/PumpComptoirDesGraines/PumpComptoirDesGraines.csproj", "src/Pump/Pumps/PumpComptoirDesGraines/"]
COPY ["src/Pump/Pump.Core/Pump.Core.csproj", "src/Pump/Pump.Core/"]
COPY ["src/LittleGarden.core/LittleGarden.Core.csproj", "src/LittleGarden.core/"]
COPY ["src/LittleGarden.Data/LittleGarden.Data.csproj", "src/LittleGarden.Data/"]
COPY ["src/Ppl.Core/Ppl.Core.csproj", "src/Ppl.Core/"]
RUN dotnet restore "src/Pump/LittleGarden.Pump/LittleGarden.Pump.csproj"
COPY . .
WORKDIR "/src/src/Pump/LittleGarden.Pump"
RUN dotnet build "LittleGarden.Pump.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LittleGarden.Pump.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get install libsnappy1v5 \
    && rm -rf /var/lib/apt/lists/* 
RUN ls -l
ENTRYPOINT ["dotnet", "LittleGarden.Pump.dll"]
