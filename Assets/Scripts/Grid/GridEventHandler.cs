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
    //..more interfaces here as required

    /// <summary>
    /// Constructor. Subscribes to the grid events straight away.
    /// </summary>
    /// <param name="gridControls">grid manipulation methods to forward events to</param>
    public GridEventHandler(IGridSystemControl gridControls)
    {
        this.gridControls = gridControls;
        //..set more interfaces here as required

        GameplayEvents.gridColUpdateEvent.AddEventListener(TerritoryChangeEventHandler);
    }

    /// <summary>
    /// Invokes SingleColControlUpdate given that territory change is made
    /// </summary>
    /// <param name="friendly"></param>
    private void TerritoryChangeEventHandler(bool friendly)
    {
        gridControls.SingleColControlUpdate(friendly);
    }

    /// <summary>
    /// Stop listening to grid events. Must be called when the grid is destroyed, otherwise the static
    /// events keep calling into a destroyed grid (e.g. after a scene change).
    /// </summary>
    public void Unsubscribe()
    {
        GameplayEvents.gridColUpdateEvent.RemoveEventListener(TerritoryChangeEventHandler);
    }
}
