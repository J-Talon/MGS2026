// Author: Jeremy
using System.Collections.Generic;
using Entities.Abstract;
using Entities.Unit;
using JetBrains.Annotations;
using Particle.Base;
using UnityEngine;

// Range-based damage and debug visualization added by Daniel
using TMPro;

/// <summary>
/// Grid based explosion resolved in layers:                                        <br></br>
/// 1) Cell allocator - pattern offsets to existing GridCells                       <br></br>
/// 2) Unit allocator - units on those cells (hostile only if there is a source)    <br></br>
/// 3) Mutate         - mutator modifies cells/units/damage                         <br></br>
/// 4) Damage         - damage each allocated unit                                  <br></br>
/// 5) VFX            - play particle on each allocated cell                        <br></br>
///                                                                                 <br></br>
/// Usage:                                                                          <br></br>
/// new Explosion(pattern, center, ParticleType.SPARK, mutator, damage, source).Detonate();
/// </summary>
public class Explosion
{
    private const float VfxDepth = -1f; // For particle system. -1 ensures it renders above the grid

    private readonly ExplosionPattern _pattern;
    private readonly ParticleType _vfx;
    private readonly IExplosionMutator _mutator;

    // Enable to view damage in debug mode
    private const bool debugViewDamage = true;

    public Vector2Int Center { get; }
    [CanBeNull] public Entity Source { get; }

    // Exposed so mutators can modify them before damage is applied
    public List<GridCell> Cells { get; } = new();
    public List<EntityUnit> Units { get; } = new();
    public float Damage { get; set; }
    public float DamageStep { get; set; } = 0f;
    public float DamageMin { get; set; } = float.MinValue;
    public float DamageMax { get; set; } = float.MaxValue;

    public Explosion(ExplosionPattern pattern, Vector2Int center, ParticleType vfx, IExplosionMutator mutator, float damage, [CanBeNull] Entity source = null)
    {
        _pattern = pattern;
        _vfx = vfx;
        _mutator = mutator;
        Center = center;
        Damage = damage;
        Source = source;
    }

    /// <summary>
    /// Run every layer of the explosion
    /// </summary>
    /// <returns>Final list of affected GridCells</returns>
    public List<GridCell> Detonate()
    {
        AllocateCells();
        AllocateUnits();
        _mutator?.Mutate(this);
        ApplyDamage();
        PlayVfx();
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
        {
            // Calculate range-based damage
            Vector2Int unitPos = unit.GetCurrentCell().GetCellPosition();
            float baseDmg = Damage + DamageStep * (Mathf.Abs(Center.x - unitPos.x) + Mathf.Abs(Center.y - unitPos.y));
            float clampedDmg = Mathf.Clamp(baseDmg, DamageMin, DamageMax);

            unit.TakeDamage(clampedDmg);

            // DEBUG: View damage applied to each unit
            if (debugViewDamage)
            {
                GameObject debugDamage = new GameObject($"{clampedDmg}");
                debugDamage.transform.SetParent(unit.transform, false);

                TextMeshPro debugDamageText = debugDamage.AddComponent<TextMeshPro>();
                debugDamageText.text = $"{clampedDmg}";
                debugDamageText.GetComponent<RectTransform>().sizeDelta = new Vector2(1f, 1f);
                debugDamageText.fontSize = 8f;
                debugDamageText.color = Color.black;
                debugDamageText.sortingOrder = 99;

                Object.Destroy(debugDamage, 2f);
            }
        }
    }

    /// <summary>
    /// Play the VFX on each allocated cell
    /// </summary>
    private void PlayVfx()
    {
        if (_vfx == null) return;
        foreach (GridCell cell in Cells)
        {
            Vector2Int position = cell.GetCellPosition();
            _vfx.Play(position.x, position.y, VfxDepth);
        }
    }
}
