using System;
using UnityEngine;
namespace Event
{

    public static class GameplayEvents
    {
        /// <summary>
        /// Groups the event relating to the Input System ||
        /// Flow Rule: Only called from InputEvents.cs -> can be listened to from anywhere (mostly listened to from PlayerController)
        /// </summary>
        public static class Input {
            public static readonly EventDispatcher<Vector2> movementInput = new EventDispatcher<Vector2>();

            public static readonly EventDispatcher<ValueTuple> interactPressed = new EventDispatcher<ValueTuple>();

            public static readonly EventDispatcher<ValueTuple> spell1Pressed = new EventDispatcher<ValueTuple>();
            public static readonly EventDispatcher<ValueTuple> spell2Pressed = new EventDispatcher<ValueTuple>();
            public static readonly EventDispatcher<ValueTuple> spell3Pressed = new EventDispatcher<ValueTuple>();

            public static readonly EventDispatcher<ValueTuple> placeTurretPressed = new EventDispatcher<ValueTuple>();
        }

        /// <summary>
        /// Group the event for the Grid ||
        /// Flow Rule: Called From (file.cs OR anywhere) -> Listened From (file.cs Or Anywhere) [Ideally either called OR listen from one file]
        /// </summary>
        public static class Grid
        {

        }

        public static readonly EventDispatcher<bool> gridColUpdateEvent = new EventDispatcher<bool>();  //< int, float param
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