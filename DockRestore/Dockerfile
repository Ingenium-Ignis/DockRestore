FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-preview-alpine AS build
ARG TARGETARCH
WORKDIR /src
COPY ["DockRestore/DockRestore.csproj", "DockRestore/"]
RUN dotnet restore "DockRestore/DockRestore.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/DockRestore"
RUN dotnet build "DockRestore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -a $TARGETARCH "DockRestore.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dockrestore.dll"]
