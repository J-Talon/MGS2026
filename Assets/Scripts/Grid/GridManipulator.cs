// Authors: [Jacky, Jeremy, Mark]
using System;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Handles every change to the grid after it is created: territory control and adding/removing columns and rows.
/// Instantiated as a base C# class by the grid manager on startup.
///
/// Knows nothing about rendering; raises Changed after every operation so the grid manager can redraw.
/// </summary>
public class GridManipulator : IGridSystemControl
{
    readonly GridState state;
    readonly GridCell cellPrefab;
    readonly Transform cellContainer;

    /// <summary>
    /// Raised after any operation that may have changed the grid's shape or ownership
    /// </summary>
    public event Action Changed;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="state">grid data this manipulator changes</param>
    /// <param name="cellPrefab">prefab cloned for every new cell</param>
    /// <param name="cellContainer">parent of every cell in the hierarchy</param>
    public GridManipulator(GridState state, GridCell cellPrefab, Transform cellContainer)
    {
        this.state = state;
        this.cellPrefab = cellPrefab;
        this.cellContainer = cellContainer;
    }

    /// <summary>
    /// Set the starting bounds and territory from the level data and create every cell
    /// </summary>
    /// <param name="levelData">grid specific level data</param>
    public void Build(GridManagerLevelData levelData)
    {
        state.XMin = levelData.xBoundingBoxMin;
        state.XMax = state.XMin + levelData.gridSize.x - 1; //-1 cause of 0 indexing
        state.YMin = levelData.yBoundingBoxMin;
        state.YMax = state.YMin + levelData.gridSize.y - 1;
        state.UncapturableColumns = levelData.uncapturableColumns;

        state.Border = ClampBorder(state.XMin + levelData.startingFriendlyColumns - 1); //-1 cause of 0 indexing

        for (int x = state.XMin; x <= state.XMax; x++)
        {
            CreateColumn(x);
        }

        Changed?.Invoke();
    }

    /// <summary>
    /// Change control of a single column
    ///
    /// by default, increases "friendly" player territory by 1 col
    /// </summary>
    /// <param name="friendly"></param>
    public void SingleColControlUpdate(bool friendly = true)
    {
        if (friendly)
        {
            if (state.Border < state.CaptureLimit)
            {
                state.Border += 1;
                SetColumnOwnership(state.Border, true);
            }
            else
            {
                Debug.Log("cannot increase friendly border control any more");
            }
        }
        else
        {
            if (state.Border >= state.XMin)
            {
                SetColumnOwnership(state.Border, false);
                state.Border -= 1;
            }
            else
            {
                Debug.Log("cannot decrease friendly border control any more");
            }
        }
        Changed?.Invoke();
    }

    /// <summary>
    /// Extend the grid by one column on the given side.
    /// A column added on the right is always enemy (it becomes part of the uncapturable columns).
    /// A column added on the left is friendly if the player owns the current leftmost column,
    /// so friendly territory always stays one contiguous block on the left.
    /// </summary>
    /// <param name="side">Edge of the grid to add the column to</param>
    public void AddGridColumn(GridSide side)
    {
        if (side == GridSide.Right)
        {
            state.XMax += 1;
        }
        else
        {
            bool isFriendly = state.Border >= state.XMin; // player owns the current leftmost column
            state.XMin -= 1;
            if (!isFriendly)
                state.Border = state.XMin - 1;
        }

        CreateColumn(side == GridSide.Right ? state.XMax : state.XMin);
        Changed?.Invoke();
    }

    /// <summary>
    /// Shrink the grid by one column on the given side. The grid always keeps at least one column.
    /// </summary>
    /// <param name="side">Edge of the grid to remove the column from</param>
    public void RemoveGridColumn(GridSide side)
    {
        if (state.XMin == state.XMax)
        {
            Debug.Log("cannot remove the last column of the grid");
            return;
        }

        if (side == GridSide.Right)
        {
            DestroyColumn(state.XMax);
            state.XMax -= 1;
        }
        else
        {
            DestroyColumn(state.XMin);
            state.XMin += 1;
        }

        // Removing on the right shifts the uncapturable columns left, removing on the left can empty
        // the player's territory; either way the border may need pulling back into range
        EnforceBorderLimits();
        Changed?.Invoke();
    }

    /// <summary>
    /// Extend the grid by one row on the given side. Ownership of the new cells follows the current border.
    /// </summary>
    /// <param name="side">Edge of the grid to add the row to</param>
    public void AddGridRow(GridRowSide side)
    {
        if (side == GridRowSide.Top)
        {
            state.YMax += 1;
            CreateRow(state.YMax);
        }
        else
        {
            state.YMin -= 1;
            CreateRow(state.YMin);
        }

        Changed?.Invoke();
    }

    /// <summary>
    /// Shrink the grid by one row on the given side. The grid always keeps at least one row.
    /// </summary>
    /// <param name="side">Edge of the grid to remove the row from</param>
    public void RemoveGridRow(GridRowSide side)
    {
        if (state.YMin == state.YMax)
        {
            Debug.Log("cannot remove the last row of the grid");
            return;
        }

        if (side == GridRowSide.Top)
        {
            DestroyRow(state.YMax);
            state.YMax -= 1;
        }
        else
        {
            DestroyRow(state.YMin);
            state.YMin += 1;
        }

        Changed?.Invoke();
    }

    // Instantiate a single grid cell at the given coordinate. Ownership follows the border.
    void CreateCell(int x, int y)
    {
        // Instantiate a new grid cell at the specified position
        GridCell newCell = Object.Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity, cellContainer);
        // Name the cell for easier identification in the hierarchy
        newCell.name = $"GridCell_{x}_{y}";
        Vector2Int position = new Vector2Int(x, y);
        newCell.SetCellPosition(position);
        newCell.isFriendly = x <= state.Border;
        // Store the cell in the grid cells dictionary for easy access later
        state.Cells[position] = newCell;
    }

    // Destroy a single grid cell at the given coordinate, if it exists
    void DestroyCell(int x, int y)
    {
        Vector2Int coordinate = new Vector2Int(x, y);
        if (state.Cells.TryGetValue(coordinate, out var cell))
        {
            Object.Destroy(cell.gameObject);
            state.Cells.Remove(coordinate);
        }
    }

    // Instantiate a full column of grid cells at the given x coordinate
    void CreateColumn(int x)
    {
        for (int y = state.YMin; y <= state.YMax; y++)
            CreateCell(x, y);
    }

    // Destroy a full column of grid cells at the given x coordinate
    void DestroyColumn(int x)
    {
        for (int y = state.YMin; y <= state.YMax; y++)
            DestroyCell(x, y);
    }

    // Instantiate a full row of grid cells at the given y coordinate
    void CreateRow(int y)
    {
        for (int x = state.XMin; x <= state.XMax; x++)
            CreateCell(x, y);
    }

    // Destroy a full row of grid cells at the given y coordinate
    void DestroyRow(int y)
    {
        for (int x = state.XMin; x <= state.XMax; x++)
            DestroyCell(x, y);
    }

    // Set the ownership of every cell in a column
    void SetColumnOwnership(int x, bool isFriendly)
    {
        for (int y = state.YMin; y <= state.YMax; y++)
            state.Cells[new Vector2Int(x, y)].isFriendly = isFriendly;
    }

    // Keep a border value between "no friendly columns" and the capture limit
    int ClampBorder(int value)
    {
        return Mathf.Max(state.XMin - 1, Mathf.Min(value, state.CaptureLimit));
    }

    // Pull the border back inside the allowed range after the grid changes shape,
    // giving any columns that fall outside it back to the enemy
    void EnforceBorderLimits()
    {
        int clamped = ClampBorder(state.Border);
        for (int x = clamped + 1; x <= state.Border; x++)
            SetColumnOwnership(x, false);
        state.Border = clamped;
    }
}
