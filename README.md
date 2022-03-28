# Dapr (Distributes Apps Runtime) - Basic Sample

[Main Site](https://dapr.io/)

## First Steps

Prepare the development Environmet.

Go to the main site and follow the getting started guide and run 

```
 dapr init
```

And wellcome to the hyperspace :)

Run docker ps, and you must see the default components defined on ~/.dapr:

- dapr_placement
- dapr_zipking
- dapr_redis: Defautl for state management and message broker

The default file seems like:

```

``` 

## First Sammple
Create a simple API Rest that save the state 

## State Smaple

---
# Notes
dapr run --app-id service1 --app-port 5062 --dapr-http-port 3500 dotnet run
http://localhost:5062/WeatherForecast
http://localhost:3500/v1.0/invoke/service1/method/weatherforecast


dapr run --app-id service2 --app-port 5044 --dapr-http-port 3501 dotnet run
http://localhost:5044/beer
http://localhost:3501/v1.0/invoke/service2/method/beer