// Authors: [Jacky, Jeremy, Mark]
using UnityEngine;

/// <summary>
/// Which horizontal edge of the grid a column operation applies to.
/// Left is the friendly home side, right is the enemy home side.
/// </summary>
public enum GridSide
{
    Left,
    Right
}

/// <summary>
/// Which vertical edge of the grid a row operation applies to.
/// </summary>
public enum GridRowSide
{
    Top,
    Bottom
}

public interface IGridSystemControl
{
    void SingleColControlUpdate(bool friendly = true);

    /// <summary>
    /// Add a new column to the grid.
    /// </summary>
    /// <param name="side">The side of the grid to add the column to.</param>
    void AddGridColumn(GridSide side);

    /// <summary>
    /// Remove a column from the grid.
    /// </summary>
    /// <param name="side">The side of the grid to remove the column from.</param>
    void RemoveGridColumn(GridSide side);

    /// <summary>
    /// Add a new row to the grid.
    /// </summary>
    /// <param name="side">The side of the grid to add the row to.</param>
    void AddGridRow(GridRowSide side);

    /// <summary>
    /// Remove a row from the grid.
    /// </summary>
    /// <param name="side">The side of the grid to remove the row from.</param>
    void RemoveGridRow(GridRowSide side);

    GridCell GetTileAtPosition(Vector2 position);
}
