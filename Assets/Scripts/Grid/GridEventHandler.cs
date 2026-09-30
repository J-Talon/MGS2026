// Authors: [Jacky, Jeremy, Mark]
using System;
using Event;
using UnityEngine;

public class GridEventHandler
{
    GridManager gridSystem;

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
    private void TerritoryChangeEventHandler(bool friendly)
    {
        gridSystem.SingleColControlUpdate(friendly);
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~GridEventHandler()
    {
        GameplayEvents.gridColUpdateEvent.RemoveEventListener(TerritoryChangeEventHandler);

    }
}
