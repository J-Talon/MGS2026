using UnityEngine;

/// <summary>
/// Interface for grid system. Contains definitions for viewing current grid system state
/// </summary>
public interface IGridSystemView
{
    /// <summary>
    /// GridCell Getter 
    /// </summary>
    /// <param name="position">Vector2Int position (should ensure its within current grid size</param>
    /// <returns>Returns grid cell given the position provided, returns null otherwise</returns>
    GridCell GetTileAtPosition(Vector2Int position);
}
