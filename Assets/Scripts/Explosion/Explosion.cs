// Author: Jeremy
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Grid based explosion resolved in layers:                                        <br></br>
/// 1) Cell allocator - pattern offsets to existing GridCells                       <br></br>
/// 2) Unit allocator - units on those cells (hostile only if there is a source)    <br></br>
/// 3) Pre Mutate     - pre-mutator step, then PreMutate VFX                        <br></br>
/// 4) Mutate         - mutator modifies cells/units/damage                         <br></br>
/// 5) Post Mutate    - post-mutator step, then PostMutate VFX                      <br></br>
/// 6) Damage         - damage each allocated unit                                  <br></br>
///                                                                                 <br></br>
/// Usage:                                                                          <br></br>
/// new Explosion(pattern, vfx, mutator, damage, source).Detonate(center);
/// </summary>
public class Explosion
{
    private readonly ExplosionPattern _pattern;
    private readonly IExplosionMutator _mutator;

    public ExplosionVFX ExplosionVFX { get; }
    public Vector2Int Center { get; set; }
    [CanBeNull] public Entity Source { get; }

    // Exposed so mutators can modify them before damage is applied
    public List<GridCell> Cells { get; } = new();
    public List<EntityUnit> Units { get; } = new();
    public float Damage { get; set; }
    public float FinalDamage { get; set; }

    public Explosion(ExplosionPattern pattern, ExplosionVFX explosionVFX, IExplosionMutator mutator, float damage, [CanBeNull] Entity source = null)
    {
        _pattern = pattern;
        _mutator = mutator;
        ExplosionVFX = explosionVFX;
        Damage = damage;
        Source = source;
    }

    /// <summary>
    /// Run every layer of the explosion
    /// </summary>
    /// <returns>Final list of affected GridCells</returns>
    public List<GridCell> Detonate(Vector2Int center)
    {
        Center = center;
        AllocateCells();
        AllocateUnits();
        _mutator?.PreMutate(this);
        _mutator?.Mutate(this);
        _mutator?.PostMutate(this);
        ApplyDamage();
        return Cells;
    }

    /// <summary>
    /// GridCell allocator. Collect the GridCells covered by the pattern including the center
    /// </summary>
    private void AllocateCells()
    {
        Cells.Clear();
        foreach (GridCell cell in _pattern.GetGridCells(Center))
        {
            if (cell && !Cells.Contains(cell)) Cells.Add(cell);
        }
    }

    /// <summary>
    /// Unit allocator. Collect units on the allocated cells. With no source every unit is hit, otherwise only hostile units.
    /// </summary>
    private void AllocateUnits()
    {
        Units.Clear();
        foreach (GridCell cell in Cells)
        {
            EntityUnit unit = cell.occupiedUnit;
            if (unit == null) continue;
            if (Source != null && !Source.IsHostile(unit)) continue;
            Units.Add(unit);
        }
    }

    /// <summary>
    /// Damage each allocated unit
    /// </summary>
    private void ApplyDamage()
    {
        foreach (EntityUnit unit in Units)
            unit.TakeDamage(FinalDamage);
    }

    //==============================
    // Example presets. Call anywhere like so - `Explosion.QUAD_3x3.Detonate(center)`

    public static readonly Explosion CROSS_3x3 = new Explosion(
        new ExplosionPatternCross(1, 1),
        ExplosionVFX.SPARK_SINGLE,
        new MutatorDamageScale(2f),
        1f
    );

    public static readonly Explosion DIAGONAL_3x3 = new Explosion(
        new ExplosionPatternDiagonal(2),
        ExplosionVFX.SPARK_SINGLE,
        new MutatorDamageScale(2f),
        1f
    );

    public static readonly Explosion LASER_DOWN_1x2 = new Explosion(
        new ExplosionPatternLaser(Vector2Int.down, 1),
        ExplosionVFX.SPARK_SINGLE,
        new MutatorDamageScale(2f),
        1f
    );

    public static readonly Explosion QUAD_3x3 = new Explosion(
        new ExplosionPatternQuad(1, 1),
        ExplosionVFX.SPARK_SINGLE,
        new MutatorDamageScale(2f),
        1f
    );

}
