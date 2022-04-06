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

Our sample is for rate Beer, we have several Beers and we can get for other service tha Beers rated and we can allow public users to set the rate.

## State Smaple


Once you have persisted the status on Redis the usually way is to connect to Redis and try to get the value:

```
  docker exec -it dapr_redis redis-cli
```
Result:
```  
  127.0.0.1:6379> keys *
1) "service2||Mahou"

  127.0.0.1:6379> hgetall service2||Mahou
1) "data"
2) "{\"name\":\"Mahou\",\"rate\":96}"
3) "version"
4) "1"

```

Also, you can abstract of Redis and invoke the state component to get the value:
``` console
curl http://localhost:3501/v1.0/state/statestore/Mahou
```
Result:
```  
{"name":"Mahou","rate":96}
```

---
# Notes

## ApiService1

Run service `ApiService1` with dapr
``` console
 dapr run --app-id service1 --app-port 5062 --dapr-http-port 3500 dotnet run
```

- [Consume Service](http://localhost:5062/WeatherForecast)
- [Consume Dapr Service](http://localhost:3500/v1.0/invoke/service1/method/weatherforecast)

## ApiService2

Run service `ApiService2` with dapr
``` console
dapr run --app-id service2 --app-port 5044 --dapr-http-port 3501 dotnet run
```

- [Consume Service](http://localhost:5044/beer)
- [Consume Dapr Service](http://localhost:3501/v1.0/invoke/service2/method/beerrate)


curl --location --request PUT "http://localhost:3501/v1.0/invoke/service2/method/beerrate" --header "Content-Type: application/json" --data-raw "{\"name\":\"Mahou\",\"rate\":96}"