# Ikv.ScreenshotWarehouse.Api
---WIP---
İstanbul Kıyamet Vakti(IKV) is first Turkish MMORPG game, still played to this date since 2007
This project is made for preserving screenshots uploaded by players to the official ikv forum. Ikv forum doesn't have an image hosting service, because of this players use popular free image hosting services which known for deleting/losing images. Every passing day more images gets deleted, most of the images before 2012 are already deleted or lost. To prevent losing images/screenshots anymore i downloaded every image on ikv forum and served them with an api and a simple website.
This project is made for mainly learning purposes.

This project has 3 parts: 
1. IkvForumImageScraper: Scrapes images from IKV forum with python, saves them to filesystem also has a script to post saved images to API.
2. Ikv.ScreenshotWarehouse.Api: An API for 
---WIP---

# HOW TO RUN

Set required env variables on your machine for docker-compose.

For Dockerfile you need to pass variables as arguments.

PostgreSQL database needs to support CITEXT
Add citext extension using code below
```
CREATE EXTENSION citext;
```

EF Core Migration instructions:
```
dotnet ef migrations add InitialMigration
dotnet ef database update || dotnet ef migrations script
```

To Get the Script of Last Migration:
```
dotnet ef migrations script 20220223182843_InitialMigration
```


## Docker Compose
```
cd .\Ikv.ScreenshotWarehouse.Api\
```
```
docker-compose up -d
```
```
docker-compose down
```
```
docker-compose start|stop
```
## Docker commands for Dockerfile V1

``` 
cd .\Ikv.ScreenshotWarehouse.Api\
```

```
docker build -f .\Ikv.ScreenshotWarehouse.Api\Dockerfile -t ikv \
--build-arg CloudinaryApiKey= \
--build-arg CloudinaryApiSecret= \
--build-arg CloudinaryCloudName= \
--build-arg IKV_JWT_SECRET= \
--build-arg IKVDBCONNSTRING='' . 
```

Use one line for docker windows...

``` 
docker build -f .\Ikv.ScreenshotWarehouse.Api\Dockerfile -t ikv --build-arg CloudinaryApiKey= --build-arg CloudinaryApiSecret= --build-arg CloudinaryCloudName= --build-arg IKV_JWT_SECRET= --build-arg IKVDBCONNSTRING='' .
```

```
docker run -it -p 80:80 --rm ikv
```
## For deployment
``` 
cd .\Ikv.ScreenshotWarehouse.Api\
```

```
git pull
```

```
docker-compose up -d --no-deps --build ss-warehouse-api
```
