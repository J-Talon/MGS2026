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
        public static class Input
        {
            public static readonly EventDispatcher<Vector2Int> movePressed = new EventDispatcher<Vector2Int>();
            public static readonly EventDispatcher<Vector2Int> moveReleased = new EventDispatcher<Vector2Int>();

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
@author Talon J         edited by Jordan (see below for changelog)
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


---CHANGELOG---

Edited by Jordan - October 14, 2026
Removed the general "move" event and replaced it with two events called "movePressed" and "moveReleased"
Reasoning for this is because i had to deal with multiple inputs. If multiple inputs were done at the same time, the character moves very incorrectly
The 2 events handling it let me break it up between a move input, and whether you pressed another movement within the movement period (has a small cooldown window)
This lets me have a buffer system 
Basically means that during the movement cooldown, if i am holding down another direction to move, it will execute that movement as soon as the cooldown ends
And to account for someone changing their buffered input to a different one, the moveReleased deals with it (hence the name)
For more details about this stuff, go to InputEvents.cs and PlayerController.cs
 */