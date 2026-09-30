using System;
using Event;
using UnityEngine;

public class GridEventHandler
{

    GridManager gridSystem;

    IGridSystemControl gridControls;

    public GridEventHandler(GridManager gridSystem)
    {
        this.gridSystem = gridSystem;
        IGridSystemControl gridControls = gridSystem;

        GameplayEvents.gridColUpdateEvent.AddEventListener(TerritoryChangeEventHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_"></param>
    private void TerritoryChangeEventHandler(bool freindly) {   
        gridControls.SingleColControlUpdate(freindly);
    }

    /// <summary>
    //Destructor
    /// </summary>
    ~GridEventHandler() 
    {
        GameplayEvents.gridColUpdateEvent.RemoveEventListener(TerritoryChangeEventHandler);

    }
}
