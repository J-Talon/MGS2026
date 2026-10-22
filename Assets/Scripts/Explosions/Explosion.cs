// Author: Jeremy
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Explosion class to handle explosions, retrieving affected cells and applying damage.
/// </summary>
public static class Explosion
{
    public static List<GridCell> CreateExplosion(GameObject source, Vector2Int center, ExplosionType explosionType, Vector2Int direction, int sizeX = 1, int sizeY = 1, float damage = 1f)
    {
        // Debug.Log($"Explosion created at {center} with size ({sizeX}, {sizeY}), damage {damage}, source: {source.name}");
        // Debug.Log($"Explosion type: {explosionType}");

        // List to store cells that will be damaged
        List<GridCell> affectedCells = ExplosionPattern.GetCells(explosionType, center, direction, sizeX, sizeY);

        // Apply damage to units occupying the each cell
        foreach (var cell in affectedCells)
        {
            ApplyDamage(cell, damage, source);
        }

        return affectedCells;
    }

    /// <summary>
    /// Apply damage to each unit on affected cells. Apply logic based on source.
    /// </summary>
    static void ApplyDamage(GridCell cell, float damage, GameObject source)
    {
        // Debug.Log($"Applying damage to: {cell.GetCellPosition()}");
        // TODO
        // get unit occupying cell
        // determine if source is friendly
        // unit.takedamage(damage)
    }
}