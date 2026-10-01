
# When to Use
Uses the event system provided by execs. The event handler should be used when its not needed to tightly couple smth to grid system, or when its appropriate to use the observer pattern (such as the grid needing to react to smth without the event invoker needing to know)

Otherwise should directly reference the grid system manager via a singleton interface to get relevant information immediately (i.e get grid cell at pos)

# Overview
Event system interactions happen primarily within "GridEventHandler.cs". This is a pure C# class (no monobehaviour) that is automatically created by the main "GridManager.cs" script on startup. 
- Did not use monobehaviour because its not really needed (i.e no actual direct Unity involvement)
- The handler class was split from the main manager class to separate responsibilities and prevent bloating the main with more code

# Code

### Class Setup & Teardown
Setup
- Constructor is passed in the associated grid manager.
- Grid manager class is casted into the respective interfaces for organization
- Subscribes to event listeners (using created handlers)
Teardown
- On grid manager reset/deletion, ideally a destructor is used, destructor will unsubscribe from events
``` c#
    /// <summary>
    /// Constructor. Only requires the grid system to be passed in. will be cast into the various interfaces as needed
    /// </summary>
    /// <param name="gridSystem">parent grid system</param>
    public GridEventHandler(GridManager gridSystem)
    {
        gridControls = gridSystem;
        gridView = gridSystem;
        //..set more interfaces here as required

        GameplayEvents.gridColUpdateEvent.AddEventListener(TerritoryChangeEventHandler);
    }
    
    //.... MORE CODE
    
    /// <summary>
    /// Destructor
    /// </summary>
    ~GridEventHandler()
    {GameplayEvents.gridColUpdateEvent.RemoveEventListener(TerritoryChangeEventHandler);
    }

```

### Handlers
For each relevant event, a handler will be used. This will pass in the event payload to the proper grid manager method as required

Ex.
```c#
    /// <summary>
    /// Invokes SingleColControlUpdate given that territory change is made
    /// </summary>
    /// <param name="_"></param>
    private void TerritoryChangeEventHandler(bool friendly)
    {
        gridControls.SingleColControlUpdate(friendly);
    }
```

