# Welcome to the WeatherTracker nanoFramework.Tarantool samples repository
This repository contains the project is for demonstration purposes only.
The solution is based on the use of the BMP280 temperature and pressure sensor.

# Usage
Important: before launching the demo project, you must [deploy the Tarantool instance](https://www.tarantool.io/en/download/os-installation/docker-hub/), for example, in docker.
You must also have an appropriate microcontroller and a properly connected BMP280 sensor:
![photo_5323712844040107582_y](https://github.com/user-attachments/assets/89d1561d-9229-4e3e-a9df-18c364889b28)

## Create docker image
To create a docker image and launch a container, you can use the [prepared files](https://github.com/RelaxSpirit/nanoFramework.Tarantool/tree/master/Samples/WeatherTracker/Tarantool) and run the necessary commands:
```
$ docker build -t tarantool-samples-app .
-------------
$ docker run -d -p 3301:3301 --name tarantool-samples-container tarantool-samples-app
```

## Build demo projects
Before assembling demo projects, it is necessary to fill in the necessary variables with up-to-date data:
```
const string Ssid = "YourSSID";
const string Password = "YourWifiPassword";
------------- And ----------------
const string TarantoolHostIp = "YourTarantoolIpAddress";
```

## Saples result
If all the steps were performed correctly, after launching the microcontroller, recordings with weather tracks should appear in the Tarantool database, which can be observed in the demo Web application:
![image](https://github.com/user-attachments/assets/7a420e5e-6bf2-4629-b8c7-939e79cf1dca)

Important: The solution is for demonstration purposes and does not handle the cleaning of outdated data in the Tarantool demo database. There is a danger that in a sufficiently long period of time, the docker container's RAM will be exhausted.
