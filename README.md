# Enttoi API for web client

[![Build status](https://ci.appveyor.com/api/projects/status/mi0xgwxrpo7kburj/branch/master?svg=true)](https://ci.appveyor.com/project/jenyayel/enttoi-api-dotnet/branch/master)

Enttoit API for retreiving current persisted state of clients and their sensors, and [SignalR](https://github.com/SignalR/SignalR) for pushing real-time updates via websocket. 

## Running in dev

1. Compile using from VS 
2. Configure environment variables located in Startup.cs
3. REST API comes with [swagger](http://swagger.io/) UI at `/swagger/ui/index` 
4. To get initial state and updates of sensors via websocket:
  * Connect to SignalR at ```/signalr```
  * Create proxy ```CommonHub```
  * Subscribe to event ```sensorStatePush```
  * Execute ```RequestInitialState``` method

5. To receive notification on entire client going offline or online:
  * Subscribe to event ```clientStatePush```


