// Author: Jeremy
using UnityEngine;
using System.Collections.Generic;

public enum ExplosionType
{
    Quad,
    Cross,
    Diagonal,
    Laser,
    // Add more explosion types as needed
}

/// <summary>
/// Defines the patterns and methods of retrieving a list of affected gridCells based on explosion pattern type
/// </summary>
public static class ExplosionPattern
{
    static readonly Vector2Int[] Diagonals = { new(1, 1), new(1, -1), new(-1, 1), new(-1, -1) };
    static readonly IGridSystemView gridManager = GridManager.instance;

    public static List<GridCell> GetCells(ExplosionType type, Vector2Int center, Vector2Int direction, int sizeX, int sizeY)
    {
        // Store list of affected GridCells
        var result = new List<GridCell>();
        // Check if center is a valid cell. otherwise continue
        CheckIfCellExist(result, center);

        // Switch statement to determine pattern method
        switch (type)
        {
            case ExplosionType.Quad: AddQuad(result, center, sizeX, sizeY); break;
            case ExplosionType.Cross: AddCross(result, center, sizeX, sizeY); break;
            case ExplosionType.Diagonal: AddRays(result, center, Diagonals, sizeX); break;
            case ExplosionType.Laser: AddRays(result, center, new[] { direction }, sizeX); break;
            default: Debug.LogWarning($"No pattern for {type}"); break;
        }

        // Return final result of affected GridCells
        return result;
    }

    /// <summary>
    /// Check if GridCell exists at given position and append the component to results
    /// </summary>
    /// <param name="result">Reference to List for appending</param>
    /// <param name="position">Target position to check if GridCell exist</param>
    static void CheckIfCellExist(List<GridCell> result, Vector2Int position)
    {
        GridCell target = gridManager.GetTileAtPosition(position);
        if (target) result.Add(target);
    }

    // Filled rectangle. sizeX/sizeY are the reach from the center, so 1,1 = 3x3.
    static void AddQuad(List<GridCell> result, Vector2Int center, int sizeX, int sizeY)
    {
        for (int x = -sizeX; x <= sizeX; x++)
            for (int y = -sizeY; y <= sizeY; y++)
            {
                if (x == 0 && y == 0) continue; // Skip center
                CheckIfCellExist(result, center + new Vector2Int(x, y));
            }

    }

    // "+" shape. sizeX = horizontal arm length, sizeY = vertical arm length.
    static void AddCross(List<GridCell> result, Vector2Int center, int sizeX, int sizeY)
    {
        for (int i = 1; i <= sizeX; i++)
        {
            CheckIfCellExist(result, center + Vector2Int.right * i);
            CheckIfCellExist(result, center + Vector2Int.left * i);
        }
        for (int i = 1; i <= sizeY; i++)
        {
            CheckIfCellExist(result, center + Vector2Int.up * i);
            CheckIfCellExist(result, center + Vector2Int.down * i);
        }
    }

    // Straight lines from the center. Used by both Diagonal (4 rays) and Laser (1 ray).
    static void AddRays(List<GridCell> result, Vector2Int center, Vector2Int[] directions, int length)
    {
        foreach (var direction in directions)
        {
            if (direction == Vector2Int.zero) continue;
            for (int i = 1; i <= length; i++) CheckIfCellExist(result, center + direction * i);
        }
    }
}
