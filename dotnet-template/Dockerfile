﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["dotnet-template.csproj", ""]
RUN dotnet restore "dotnet-template.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "dotnet-template.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-template.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-template.dll"]
