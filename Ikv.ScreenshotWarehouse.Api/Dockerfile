FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
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
