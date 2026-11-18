// Author: Daniel

using UnityEngine;
using Entities.Abstract;
using Particle.Base;

public class ExplosionLaserPlayer : Explosion
{
    /// <summary>
    /// Creates a laser that fires from the origin to the right of the grid.
    /// </summary>
    /// <param name="origin">The (x, y) coordinate where the laser originated from.</param>
    /// <param name="damage">The amount of damage dealt on contact with any part of the laser.</param>
    /// <param name="source">The source of the laser.</param>
    /// <returns></returns>
    public static ExplosionLaserPlayer Create(Vector2Int origin, float damage, Entity source = null)
    {
        GridManager gm = Object.FindFirstObjectByType<GridManager>();
        int maxX = gm.GetGridBounds().max.x;
        int laserWidth = maxX - origin.x;

        return new ExplosionLaserPlayer(
            new ExplosionPatternLazer(Vector2Int.right, laserWidth),
            origin,
            ParticleType.SPARK,
            null,
            damage,
            null);
    }

    private ExplosionLaserPlayer(ExplosionPattern pattern, Vector2Int center, ParticleType vfx, IExplosionMutator mutator, float damage, Entity source = null) : base(pattern, center, vfx, mutator, damage, source) { }
}