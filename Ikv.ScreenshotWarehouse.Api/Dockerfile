#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80

ARG CloudinaryApiKey
ARG CloudinaryApiSecret
ARG CloudinaryCloudName
ARG ECOMMERCE_JWT_SECRET
ARG IKVDBCONNSTRING

ENV CloudinaryApiKey=$CloudinaryApiKey
ENV CloudinaryApiSecret=$CloudinaryApiSecret
ENV CloudinaryCloudName=$CloudinaryCloudName
ENV ECOMMERCE_JWT_SECRET=$ECOMMERCE_JWT_SECRET
ENV IKVDBCONNSTRING=$IKVDBCONNSTRING

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
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