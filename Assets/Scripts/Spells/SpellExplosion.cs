//author - Jordan -> Nov 08, 2026

using Particle.Base;
using UnityEngine;

//the explosion (aka the actual attack)
public class SpellExplosion
{
    private SpellShape shape; //explosion shape, see SpellShape.cs and ExplosionPattern.cs
    private int size; //how big the explosion is. The way it grows depends on the explosionPattern. Again, see the above files or the explosion task document for better understanding
    private float damage;
    private IExplosionMutator mutator;

    //the settings for the explosion
    public SpellExplosion(SpellShape explosionShape, int sizeIncrement, float explosionDamage)
    {
        shape = explosionShape;
        size = sizeIncrement;
        damage = explosionDamage;
    }

    //Detonate the explosion after setting the pattern
    public void StartExplosion(Vector2Int origin, SpellCastingData castingData)
    {
        ExplosionPattern pattern = shape(castingData.direction, size);
        new Explosion(pattern, origin, ParticleType.SPARK, mutator, damage, castingData.spellCaster).Detonate();
    }
}