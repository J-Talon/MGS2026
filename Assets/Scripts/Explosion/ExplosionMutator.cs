// Author: Jeremy

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

/// <summary>
/// Modifies an explosion after its cells and units are allocated and before damage is applied
/// </summary>
public interface IExplosionMutator
{
    public void PreMutate(Explosion explosion);
    public void Mutate(Explosion explosion);
    public void PostMutate(Explosion explosion);
}

/// <summary>
/// Multiplies the explosion damage by a fixed amount
/// </summary>
public class MutatorDamageScale : IExplosionMutator
{
    private readonly float _multiplier;
    public MutatorDamageScale(float multiplier) { _multiplier = multiplier; }
    public void PreMutate(Explosion explosion) { explosion.ExplosionVFX?.Play(ExplosionVFXStep.PreMutate, explosion.Cells); }
    public void Mutate(Explosion explosion) { explosion.Damage *= _multiplier; }
    public void PostMutate(Explosion explosion) { explosion.ExplosionVFX?.Play(ExplosionVFXStep.PostMutate, explosion.Cells); }
}
