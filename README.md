# Enttoi API

|Branch|Appveyor|
|------|:------:|
|master|[![Build status](https://ci.appveyor.com/api/projects/status/mi0xgwxrpo7kburj/branch/master?svg=true)](https://ci.appveyor.com/project/jenyayel/enttoi-api-dotnet/branch/master)|
|dev   |[![Build status](https://ci.appveyor.com/api/projects/status/mi0xgwxrpo7kburj/branch/dev?svg=true)](https://ci.appveyor.com/project/jenyayel/enttoi-api-dotnet/branch/dev)|

Enttoit API for retreiving current persisted state of clients and their sensors, and [SignalR](https://github.com/SignalR/SignalR) for pushing real-time updates via websocket. To be used by different clients - web, hybrid and native applications.

## Running in dev

1. Compile using from VS 
2. Configure environment variables located in Startup.cs
3. REST API comes with [swagger](http://swagger.io/) UI at `/swagger/ui/index` 
4. To get initial state and updates of sensors via websocket:
  * Create proxy ```CommonHub```
  * Subscribe to event ```sensorStatePush``` (before connecting to proxy)
5. To receive notification on entire client going offline or online:
  * Subscribe to event ```clientStatePush``` (before connecting to proxy)
6. To receive a number of connected users
  * Subscribe to event ```onlineUsersPush``` (before connecting to proxy)


