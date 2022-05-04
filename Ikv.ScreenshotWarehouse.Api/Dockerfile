#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

ARG CloudinaryApiKey
ARG CloudinaryApiSecret
ARG CloudinaryCloudName
ARG IKV_JWT_SECRET
ARG IKVDBCONNSTRING

ENV CloudinaryApiKey=$CloudinaryApiKey
ENV CloudinaryApiSecret=$CloudinaryApiSecret
ENV CloudinaryCloudName=$CloudinaryCloudName
ENV IKV_JWT_SECRET=$IKV_JWT_SECRET
ENV IKVDBCONNSTRING=$IKVDBCONNSTRING

ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Ikv.ScreenshotWarehouse.Api/Ikv.ScreenshotWarehouse.Api.csproj", "Ikv.ScreenshotWarehouse.Api/"]
RUN dotnet restore "Ikv.ScreenshotWarehouse.Api/Ikv.ScreenshotWarehouse.Api.csproj"
COPY . .
WORKDIR "/src/Ikv.ScreenshotWarehouse.Api"
RUN dotnet build "Ikv.ScreenshotWarehouse.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ikv.ScreenshotWarehouse.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ikv.ScreenshotWarehouse.Api.dll"]
