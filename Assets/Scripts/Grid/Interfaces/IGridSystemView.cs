using UnityEngine;
using System.Collections.Generic;
using System;
using Entities.Unit;

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

    /// <summary>
    /// Function to get the bounds of the friendly grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    (Vector2Int min, Vector2Int max) GetFriendlyGridBounds();

    /// <summary>
    /// Function to get the bounds of the enemy grid
    /// </summary>
    /// <returns>Bounds of the grid as a ValueTuple with the min and max bounds as Vector2Int: (Min Bounds, Max Bounds)</returns>
    (Vector2Int min, Vector2Int max) GetEnemyGridBounds();

    /// <summary>
    /// Does a linear search to find every entity matching a criteria
    /// </summary>
    /// <param name="position">XY coordinate of GridCell</param>
    /// <returns>GridCell object</returns>
    List<(Vector2Int, EntityUnit)> FindAllEntities(Func<EntityUnit, bool> criteria);

    /// <summary>
    /// </summary>
    /// <returns>If coordinate exists within grid bounds</returns>
    bool WithinGrid(Vector2Int position);


    /// <returns>If coordinate exists within grid bounds</returns>
    bool WithinGridSmooth(Vector2 position);
}
