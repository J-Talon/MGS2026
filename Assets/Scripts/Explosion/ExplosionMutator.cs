// Author: Jeremy
// MutatorDamageRange added by Daniel

/*
HOW TO CREATE A NEW EXPLOSION MUTATOR
1) Create a class implementing IExplosionMutator, e.g. MutatorXXX
2) In Mutate, modify the explosion's Cells, Units or Damage
    - Cells: GridCells that receive VFX
    - Units: EntityUnits that receive damage
3) Pass it to an explosion:
    new Explosion(pattern, center, vfx, new MutatorXXX(...), damage, source).Detonate();

Mutate runs after cells and units are allocated, and before damage and VFX are applied.
Cells and Units are not kept in sync, so if you remove a cell, also remove its unit if it should not be damaged.

See Explosion.cs
*/

using Effect.EntityEffect;

/// <summary>
/// Modifies an explosion after its cells and units are allocated and before damage is applied
/// </summary>
public interface IExplosionMutator
{
    public void Mutate(Explosion explosion);
}

/// <summary>
/// Multiplies the explosion damage by a fixed amount
/// </summary>
public class MutatorDamageScale : IExplosionMutator
{
    private readonly float _multiplier;

    public MutatorDamageScale(float multiplier)
    {
        _multiplier = multiplier;
    }

    public void Mutate(Explosion explosion)
    {
        explosion.Damage *= _multiplier;
    }
}

/// <summary>
/// Adds <c>step</c> additional damage to each affected cell based on how far away the cell is from the center,
/// while constraining the damage to <c>min</c> and <c>max</c>
/// </summary>

//Author: Daniel
public class MutatorDamageRange : IExplosionMutator
{
    private readonly float? _min, _max;
    private readonly float _step;

    public MutatorDamageRange(float? min, float? max, float step)
    {
        _min = min;
        _max = max;
        _step = step;
    }

    public void Mutate(Explosion explosion)
    {
        if (_min != null) explosion.DamageMin = (float)_min;
        if (_max != null) explosion.DamageMax = (float)_max;
        explosion.DamageStep = _step;
    }
}

/// <summary>
/// Applies a three-second ensnare to all units selected by the explosion.
/// </summary>

//Author: Sayeed
public class MutatorEnsnare : IExplosionMutator
{
    public void Mutate(Explosion explosion)
    {
        foreach (var unit in explosion.Units)
        {
            unit.AddEffect(new EffectEnsnared(unit, 3));
        }
    }
} 
