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
    
    /// <summary>
    /// Function to get a entity at the given grid cell
    /// It is a wrapper function using GetTileAtPosition
    /// </summary>
    /// <param name="position">Vector2Int position (should ensure its within current grid size</param>
    /// <returns>Entity if one exists there (using a snappy movement type), null otherwise</returns>
    EntityUnit GetEntityAtTilePosition(Vector2Int position);

    /// <summary>
    /// Function to get the bounds of the grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    (Vector2Int min, Vector2Int max) GetGridBounds();

    /// Function to get the bounds of the friendly grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    (Vector2Int min, Vector2Int max) GetFriendlyGridBounds();

    /// Function to get the bounds of the enemy grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    (Vector2Int min, Vector2Int max) GetEnemyGridBounds();
}
