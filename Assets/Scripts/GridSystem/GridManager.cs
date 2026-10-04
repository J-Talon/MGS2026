using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GridManager : MonoBehaviour
{
    // instantiate as static singleton
    public static GridManager Instance {get; private set;}

    // inspector-settable grid size
    public Vector2Int gridDimensions = new Vector2Int(8,5);
    public int columnsOwned = 4;

    // mapping of tiles and if they are player-owned
    private bool[] ownedColumns;

    public bool IsTileOwned(Vector2Int targetTile)
    {
        return ownedColumns[targetTile.x];
    }

    public void SetColumns(int column, bool newState = true, bool shouldSweep = false)
    {
        // avoid out of bounds index
        Mathf.Clamp(column, 0, ownedColumns.Length);

        // set a single column as owned
        if (!shouldSweep)
        {
            ownedColumns.SetValue(newState, column);
            return;
        }

        // set all columns left of target (inclusive)
        for (int i = 0; i < column; i++)
        {
            if (ownedColumns[i] == newState)
            {
                continue;
            }

            ownedColumns.SetValue(newState, i);
        }
    }
}