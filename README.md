# Ikv.ScreenshotWarehouse.Api

Backend for a fan made screenshot warehouse of a legendary turkish mmorpg game

#HOW TO RUN

Docker command for Dockerfile V1

- cd .\Ikv.ScreenshotWarehouse.Api\

- docker build -f .\Ikv.ScreenshotWarehouse.Api\Dockerfile -t ikv \
--build-arg CloudinaryApiKey= \
--build-arg CloudinaryApiSecret= \
--build-arg CloudinaryCloudName= \
--build-arg ECOMMERCE_JWT_SECRET= \
--build-arg IKVDBCONNSTRING='' .

Use one line in windows...

- docker build -f .\Ikv.ScreenshotWarehouse.Api\Dockerfile -t ikv --build-arg CloudinaryApiKey= --build-arg CloudinaryApiSecret= --build-arg CloudinaryCloudName= --build-arg ECOMMERCE_JWT_SECRET= --build-arg IKVDBCONNSTRING='' .


- docker run -it -p 80:80 --rm ikv