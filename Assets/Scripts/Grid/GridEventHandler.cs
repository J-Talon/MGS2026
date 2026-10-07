// Authors: [Jacky, Jeremy, Mark]
using Event;

/// <summary>
/// Event handler for grid system. Instantiated as a base C# class by the grid system on startup.
/// Hooks into all public grid system methods via the various grouped methods in the interfaces.
/// 
/// When events are called this handler will invoke the required methods as required and pass along the data specified
/// </summary>
public class GridEventHandler
{
    IGridSystemControl gridControls;
    IGridSystemView gridView;
    //..more interfaces here as required

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

    /// <summary>
    /// Invokes SingleColControlUpdate given that territory change is made
    /// </summary>
    /// <param name="_"></param>
    private void TerritoryChangeEventHandler(bool friendly)
    {
        gridControls.SingleColControlUpdate(friendly);
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~GridEventHandler()
    {
        GameplayEvents.gridColUpdateEvent.RemoveEventListener(TerritoryChangeEventHandler);
    }
}
