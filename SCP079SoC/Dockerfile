FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SCP079SoC/SCP079SoC.csproj", "SCP079SoC/"]
RUN dotnet restore "SCP079SoC/SCP079SoC.csproj"
COPY . .
WORKDIR "/src/SCP079SoC"
RUN dotnet build "SCP079SoC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SCP079SoC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SCP079SoC.dll"]
