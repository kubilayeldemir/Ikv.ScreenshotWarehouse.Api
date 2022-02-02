# Ikv.ScreenshotWarehouse.Api

Backend for a fan made screenshot warehouse of a legendary turkish mmorpg game

# HOW TO RUN

Set required env variables on your machine for docker-compose.

For Dockerfile you need to pass variables as arguments.

PostgreSQL database needs to support CITEXT
Add citext extension using code below
```
CREATE EXTENSION citext;
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
