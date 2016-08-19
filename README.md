#Inversion.Ultrastructure
This is a class library for Inversion which allows a pipeline in one thread to publish events to other connected pipelines, possibly on other machines.
Publishing is done via a pub-sub pattern, hidden behind the IPubSubClient interface.
Currently there is a Redis client for this, but other transports will be fairly straightforward to add.

#Submodule setup
I do something a bit niche with submodules, so I would recommend not doing the --recursive thing because that will cause mass duplication of repository contents.
Simply running the following instructions should mean you get a single copy of each submodule in the root "submodules" folder.
```
cd submodules
git submodule init
git submodule update

```

#Demos

To run the demos you will need to have a Redis server of some sort.
I use the Windows version when I'm testing locally.
The default Redis address is localhost:6379. The default Redis database number is 15.
You can change the Redis server address, port and database number in the App.config files in the demo projects.

##What to expect
Both demos feature a primary message pump that publishes a particular event ("hello-from-pump") to a specific channel ("global").
The first listener will react to "hello-from-pump" events by logging a message to the console, changing a parameter in the event and publishing a new event ("hello-from-listener1") to the same channel.
The second listener will react to "hello-from-listener1" events by logging a message to the console.

###Ultrastructure.Demo1

Single process demonstration which launches 3 tasks - one pump, two listeners - that communicate in a cascade.

###Ultrastructure.Demo2

Executable takes a single parameter.

Start the primary event pump:
```
Ultrastructure.Demo2.exe pump
```

Start the first listener:
```
Ultrastructure.Demo2.exe listener1
```

Start the second listener:
```
Ultrastructure.Demo2.exe listener2
```
