// Author: Jeremy
using System.Collections.Generic;
using UnityEngine;

/*
HOW TO CREATE A NEW EXPLOSION PATTERN
1) Create a class inheriting ExplosionPattern, e.g. ExplosionPatternXXX
2) Override GetOffsets and yield the cell offsets relative to the center (center is already included)
3) Pass it to an explosion:
    new Explosion(new PatternXXX(...), center, vfx, mutator, damage, source).Detonate();

See Explosion.cs
*/

/// <summary>
/// Defines the shape of an explosion and retrieves the GridCells it covers
/// </summary>
public abstract class ExplosionPattern
{
    /// <summary>
    /// Offsets relative to the center, excluding the center itself
    /// </summary>
    protected abstract IEnumerable<Vector2Int> GetOffsets();

    /// <summary>
    /// GridCells covered by the pattern around the center, including the center. Positions outside the grid are skipped.
    /// </summary>
    public IEnumerable<GridCell> GetGridCells(Vector2Int center)
    {
        IGridSystemView gridManager = GridManager.instance;

        GridCell centerCell = gridManager.GetTileAtPosition(center);
        if (centerCell) yield return centerCell;

        foreach (Vector2Int offset in GetOffsets())
        {
            GridCell cell = gridManager.GetTileAtPosition(center + offset);
            if (cell) yield return cell;
        }
    }
}

/// <summary>
/// Filled rectangle. sizeX/sizeY are the reach from the center, so 1,1 = 3x3.
/// </summary>
public class ExplosionPatternQuad : ExplosionPattern
{
    private readonly int _sizeX;
    private readonly int _sizeY;

    public ExplosionPatternQuad(int sizeX = 1, int sizeY = 1)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
    }

    protected override IEnumerable<Vector2Int> GetOffsets()
    {
        for (int x = -_sizeX; x <= _sizeX; x++)
            for (int y = -_sizeY; y <= _sizeY; y++)
            {
                if (x == 0 && y == 0) continue; // Skip center
                yield return new Vector2Int(x, y);
            }
    }
}

/// <summary>
/// "+" shape. sizeX = horizontal arm length, sizeY = vertical arm length.
/// </summary>
public class ExplosionPatternCross : ExplosionPattern
{
    private readonly int _sizeX;
    private readonly int _sizeY;

    public ExplosionPatternCross(int sizeX = 1, int sizeY = 1)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
    }

    protected override IEnumerable<Vector2Int> GetOffsets()
    {
        for (int i = 1; i <= _sizeX; i++)
        {
            yield return Vector2Int.right * i;
            yield return Vector2Int.left * i;
        }
        for (int i = 1; i <= _sizeY; i++)
        {
            yield return Vector2Int.up * i;
            yield return Vector2Int.down * i;
        }
    }
}

/// <summary>
/// "X" shape. length = reach of each diagonal arm.
/// </summary>
public class ExplosionPatternDiagonal : ExplosionPattern
{
    private static readonly Vector2Int[] Diagonals = { new(1, 1), new(1, -1), new(-1, 1), new(-1, -1) };

    private readonly int _length;

    public ExplosionPatternDiagonal(int length = 1)
    {
        _length = length;
    }

    protected override IEnumerable<Vector2Int> GetOffsets()
    {
        foreach (Vector2Int direction in Diagonals)
            for (int i = 1; i <= _length; i++)
                yield return direction * i;
    }
}

/// <summary>
/// Single straight line from the center in a direction. length = number of cells past the center.
/// </summary>
public class ExplosionPatternLaser : ExplosionPattern
{
    private readonly Vector2Int _direction;
    private readonly int _length;

    public ExplosionPatternLaser(Vector2Int direction, int length = 1)
    {
        _direction = direction;
        _length = length;
    }

    protected override IEnumerable<Vector2Int> GetOffsets()
    {
        if (_direction == Vector2Int.zero) yield break;
        for (int i = 1; i <= _length; i++)
            yield return _direction * i;
    }
}
