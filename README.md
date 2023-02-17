# Ikv.ScreenshotWarehouse.Api

İstanbul Kıyamet Vakti(IKV) is first Turkish MMORPG game, still played to this date since 2007
This project is made for preserving screenshots uploaded by players to the official game forum. Ikv's forum website doesn't have an image hosting service, because of this players use popular free image hosting services which known for deleting/losing images. Every passing day more images gets deleted, most of the images before 2012 are already deleted or lost. To prevent losing images/screenshots anymore i downloaded every image on ikv forum and served them with an api and a simple website.
This project is made for mainly learning purposes.

Project has 4 parts: 
1. [IkvForumImageScraper](https://github.com/kubilayeldemir/IkvForumImageScraper): Scrapes images from IKV forum with python beautifulsoup library, saves images to filesystem.There is also a simple script to read images from the filesystem, check if the image is corrupted, and send healthy images to Ikv.ScreenshotWarehouse.Api.
2. Ikv.ScreenshotWarehouse.Api(this project): An API for letting users save images and serves saved images to clients. Images are saved to PostgreSQL and used to be hosted on Cloudinary, after realizing the free quota of Cloudinary is not going to be sufficient, images are saved to the filesystem to be served by nginx. Deployed to Oracle Cloud VM via docker.
3. [ikv-nginx](https://github.com/kubilayeldemir/ikv-nginx): Acts as a reverse proxy, works on port 80 which is open to public, redirects urls with /api to Ikv.ScreenshotWarehouse.Api, else serves images from the filesystem which contains images saved by API, a simple image host service. Deployed to Oracle Cloud VM via docker. I used Cloudflare to manage the domain and enable SSL(flexible ssl).
4. [Ikv.ScreenshotWarehouse.UI](https://github.com/kubilayeldemir/Ikv.ScreenshotWarehouse.UI): A simple UI made with nuxt.js, consumes the api and serves images to users. Server-side rendering was used. Deployed to Vercell.


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
