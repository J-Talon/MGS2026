using System;
namespace Event
{


    public static class GameplayEvents
    {
        
    }
    
}



/*
@author Talon J
How to use the event system:

1: Declare a listener function where you need it.

==================
private void MyListenerFunction(ValueTuple _) {   // <== use ValueTuple _ if you need no parameters
//do stuff here

}

private void MyListenerFunction((int a, float b) event) {   // <== Example with an int and float parameter
//do stuff here

}

==================

2: Declare an event in the GameplayEvents.class

//Sample event declarations
public static readonly EventDispatcher<ValueTuple> playerEvent = new EventDispatcher<ValueTuple>();  //< no params

public static readonly EventDispatcher<(int, float)> playerEvent = new EventDispatcher<(int, float)>();  //< int, float param

==================


3: Register the listener function
GameplayEvents.playerEvent.AddEventListener(MyListenerFunction);

==================


4: Call the event
GameplayEvents.playerEvent.callEvent(ValueTuple.create());  <== no arguments

GameplayEvents.playerEvent.callEvent(new ValueTuple<int, float>(1,1.23));  <== int, float argument

==================


5:
Remember to unregister the event when the object is destroyed, otherwise you will get a null reference exception
(this usually happens when you forget, and then change scenes)

GameplayEvents.playerEvent.RemoveEventListener(MyListenerFunction);



 */