// Authors: [Jacky, Jeremy, Mark]
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Runtime data of the grid system: bounds, territory border and the cells themselves.
/// Shared between GridManager (reads it to render and answer queries) and GridManipulator (changes it).
/// Holds no logic of its own.
/// </summary>
public class GridState
{
    /// <summary>
    /// Runtime bounds of the grid (inclusive). Copied from the level data on startup so that
    /// adding/removing columns and rows at runtime never modifies the ScriptableObject asset.
    /// </summary>
    public int XMin, XMax, YMin, YMax;

    /// <summary>
    /// The bordering column of control (W.R.T player).
    /// Column x is friendly if x <= Border. Border == XMin - 1 means no friendly columns.
    /// </summary>
    public int Border;

    /// <summary>
    /// Number of rightmost columns that always belong to the enemy and can never be captured
    /// </summary>
    public int UncapturableColumns;

    /// <summary>
    /// The rightmost column the player is allowed to own.
    /// Columns past this are permanently enemy territory and can never be captured.
    /// </summary>
    public int CaptureLimit => XMax - UncapturableColumns;

    /// <summary>
    /// Every cell in the grid, keyed by its grid coordinate
    /// </summary>
    public readonly Dictionary<Vector2Int, GridCell> Cells = new Dictionary<Vector2Int, GridCell>();
}
